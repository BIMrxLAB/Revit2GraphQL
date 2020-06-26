#region Copyright
// (C) Copyright 2003-2016 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Copyright

#region Namespaces
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
#endregion // Namespaces

namespace TraverseAllSystems
{
    /// <summary>
    /// TreeNode object representing a system element
    /// </summary>
    public class TreeNode
    {
        #region JSON Output Format Strings
        /// <summary>
        /// Format a tree node to JSON storing parent id 
        /// in child node for bottom-up structure.
        /// </summary>
        const string _json_format_to_store_parent_in_child
          = "{{"
          + "\"id\" : {0}, "
          + "\"{1}\" : \"{2}\", "
          + "\"parent\" : {3}}}";

        /// <summary>
        /// Format a tree node to JSON storing a 
        /// hierarchical tree of children ids in parent 
        /// for top-down structure.
        /// </summary>
        const string _json_format_to_store_children_in_parent
          = "{{"
          + "\"id\" : \"{0}\", "
          + "\"{1}\" : \"{2}\", "
          + "\"children\" : [{3}]}}";

        /// <summary>
        /// Create a new JSON parent node for the top-down graph.
        /// </summary>
        public static string CreateJsonParentNode(
          string id,
          string label,
          string json_kids)
        {
            return string.Format(
              _json_format_to_store_children_in_parent,
              id, Options.NodeLabelTag, label, json_kids);
        }

        /// <summary>
        /// Create a new JSON parent node for the top-down graph.
        /// </summary>
        public static string CreateJsonParentNode(
          string id,
          string label,
          string[] json_kids)
        {
            return CreateJsonParentNode(id, label,
              string.Join(",", json_kids));
        }
        #endregion // JSON Output Format Strings

        #region Member variables
        /// <summary>
        /// Id of the element
        /// </summary>
        private Autodesk.Revit.DB.ElementId m_Id;

        /// <summary>
        /// Flow direction of the node
        /// For the starting element of the traversal, the direction will be the same as the connector
        /// connected to its following element; Otherwise it will be the direction of the connector connected to
        /// its previous element
        /// </summary>
        private FlowDirectionType m_direction;

        /// <summary>
        /// The parent node of the current node.
        /// </summary>
        private TreeNode m_parent;

        /// <summary>
        /// The connector of the previous element to which current element is connected
        /// </summary>
        private Connector m_inputConnector;

        /// <summary>
        /// The first-level child nodes of the current node
        /// </summary>
        private List<TreeNode> m_childNodes;

        /// <summary>
        /// Active document of Revit
        /// </summary>
        private Document m_document;
        #endregion

        #region Properties
        /// <summary>
        /// Id of the element
        /// </summary>
        public ElementId Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// Flow direction of the node
        /// </summary>
        public FlowDirectionType Direction
        {
            get
            {
                return m_direction;
            }
            set
            {
                m_direction = value;
            }
        }

        /// <summary>
        /// Gets and sets the parent node of the current node.
        /// </summary>
        public TreeNode Parent
        {
            get
            {
                return m_parent;
            }
            set
            {
                m_parent = value;
            }
        }

        /// <summary>
        /// Gets and sets the first-level child nodes of the current node
        /// </summary>
        public List<TreeNode> ChildNodes
        {
            get
            {
                return m_childNodes;
            }
            set
            {
                m_childNodes = value;
            }
        }

        /// <summary>
        /// The connector of the previous element to which current element is connected
        /// </summary>
        public Connector InputConnector
        {
            get
            {
                return m_inputConnector;
            }
            set
            {
                m_inputConnector = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="doc">Revit document</param>
        /// <param name="id">Element's Id</param>
        public TreeNode(Document doc, ElementId id)
        {
            m_document = doc;
            m_Id = id;
            m_childNodes = new List<TreeNode>();
        }
        #endregion // Constructor

        #region GetElementById
        /// <summary>
        /// Get Element by its Id
        /// </summary>
        /// <param name="eid">Element's Id</param>
        /// <returns>Element</returns>
        private Element GetElementById(ElementId eid)
        {
            return m_document.GetElement(eid);
        }
        #endregion // GetElementById

        #region JSON Output
        public static string GetName(Element e)
        {
            return Util.ElementDescription(e)
              .Replace("\"", "\\\"");
        }

        public static string GetId(Element e)
        {
            return Options.StoreUniqueId
              ? e.UniqueId
              : e.Id.IntegerValue.ToString();
        }

        /// <summary>
        /// Add JSON strings representing all children 
        /// of this node to the given collection.
        /// </summary>
        public void DumpToJsonBottomUp(
          List<string> json_collector,
          string parent_id)
        {
            Element e = GetElementById(m_Id);
            string id = GetId(e);

            string json = string.Format(
              _json_format_to_store_parent_in_child, id,
              Options.NodeLabelTag, GetName(e),
              parent_id);

            json_collector.Add(json);

            foreach (TreeNode node in m_childNodes)
            {
                node.DumpToJsonBottomUp(json_collector, id);
            }
        }

        /// <summary>
        /// Return a JSON string representing this node and
        /// including the recursive hierarchical graph of 
        /// all its all children.
        /// </summary>
        public string DumpToJsonTopDown()
        {
            Element e = GetElementById(m_Id);

            List<string> json_collector = new List<string>();

            foreach (TreeNode child in m_childNodes)
            {
                json_collector.Add(child.DumpToJsonTopDown());
            }

            string json = CreateJsonParentNode(GetId(e),
              GetName(e), json_collector.ToArray());

            // Todo: properties print

            return json;
        }
        #endregion // JSON Output

        #region XML Output
        /// <summary>
        /// Dump the node into XML file.
        /// </summary>
        public void DumpIntoXML(XmlWriter writer)
        {
            // Write node information
            Element element = GetElementById(m_Id);
            FamilyInstance fi = element as FamilyInstance;
            if (fi != null)
            {
                MEPModel mepModel = fi.MEPModel;
                String type = String.Empty;
                if (mepModel is MechanicalEquipment)
                {
                    type = "MechanicalEquipment";
                    writer.WriteStartElement(type);
                }
                else if (mepModel is MechanicalFitting)
                {
                    MechanicalFitting mf = mepModel as MechanicalFitting;
                    type = "MechanicalFitting";
                    writer.WriteStartElement(type);
                    writer.WriteAttributeString("Category", element.Category.Name);
                    writer.WriteAttributeString("PartType", mf.PartType.ToString());
                }
                else
                {
                    type = "FamilyInstance";
                    writer.WriteStartElement(type);
                    writer.WriteAttributeString("Category", element.Category.Name);
                }

                writer.WriteAttributeString("Name", element.Name);
                writer.WriteAttributeString("Id", element.Id.IntegerValue.ToString());
                writer.WriteAttributeString("Direction", m_direction.ToString());
                writer.WriteEndElement();
            }
            else
            {
                String type = element.GetType().Name;

                writer.WriteStartElement(type);
                writer.WriteAttributeString("Name", element.Name);
                writer.WriteAttributeString("Id", element.Id.IntegerValue.ToString());
                writer.WriteAttributeString("Direction", m_direction.ToString());
                writer.WriteEndElement();
            }

            foreach (TreeNode node in m_childNodes)
            {
                if (m_childNodes.Count > 1)
                {
                    writer.WriteStartElement("Path");
                }

                node.DumpIntoXML(writer);

                if (m_childNodes.Count > 1)
                {
                    writer.WriteEndElement();
                }
            }
        }
        #endregion // XML Output

        public void CollectUniqueIds(StringBuilder sb)
        {
            Element element = GetElementById(this.m_Id);


            sb.Append("\"" + GetId(element) + "\",");
            foreach (TreeNode node in this.m_childNodes)
            {
                node.CollectUniqueIds(sb);
            }
        }
    }

    /// <summary>
    /// Data structure of the system traversal
    /// </summary>
    public class TraversalTree
    {
        public static byte MECHANICAL_MASK = 1;
        public static byte ELECTRICAL_MASK = 2;
        public static byte PIPING_MAKS = 4;

        #region Member variables
        /// <summary>
        /// Active Revit document
        /// </summary>
        private Document m_document;

        /// <summary>
        /// The MEP system of the traversal
        /// </summary>
        private MEPSystem m_system;

        /// <summary>
        /// Flag whether the MEP system is mechanical or piping
        /// </summary>
        private Boolean m_isMechanicalSystem;

        /// <summary>
        /// The starting element node
        /// </summary>
        private TreeNode m_startingElementNode;

        /// <summary>
        /// Map element id integer to the number 
        /// of times the element has been visited.
        /// </summary>
        Dictionary<int, int> _visitedElementCount;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activeDocument">Revit document</param>
        /// <param name="system">The MEP system to traverse</param>
        public TraversalTree(MEPSystem system)
        {
            m_document = system.Document;
            m_system = system;
            m_isMechanicalSystem = (system is MechanicalSystem);
            _visitedElementCount = new Dictionary<int, int>();
        }
        #endregion // Constructor

        #region Tree Construction
        /// <summary>
        /// Traverse the system
        /// </summary>
        public bool Traverse()
        {
            // Get the starting element node
            m_startingElementNode = GetStartingElementNode();

            if (null != m_startingElementNode)
            {
                // Traverse the system recursively
                Traverse(m_startingElementNode);
            }
            return null != m_startingElementNode;
        }

        /// <summary>
        /// Get the starting element node.
        /// If the system has base equipment then get it;
        /// Otherwise get the owner of the open connector in the system
        /// </summary>
        /// <returns>The starting element node</returns>
        private TreeNode GetStartingElementNode()
        {
            TreeNode startingElementNode = null;

            FamilyInstance equipment = m_system.BaseEquipment;
            //
            // If the system has base equipment then get it;
            // Otherwise get the owner of the open connector in the system
            if (equipment != null)
            {
                startingElementNode = new TreeNode(m_document, equipment.Id);
            }
            else
            {
                Element root = GetOwnerOfOpenConnector();
                if (null != root)
                {
                    startingElementNode = new TreeNode(m_document, root.Id);
                }
            }

            if (null != startingElementNode)
            {
                startingElementNode.Parent = null;
                startingElementNode.InputConnector = null;
            }

            return startingElementNode;
        }

        /// <summary>
        /// Get the owner of the open connector as the starting element
        /// </summary>
        /// <returns>The owner</returns>
        private Element GetOwnerOfOpenConnector()
        {
            Element element = null;

            //
            // Get an element from the system's terminals
            ElementSet elements = m_system.Elements;
            foreach (Element ele in elements)
            {
                element = ele;
                break;
            }

            // Get the open connector recursively
            Connector openConnector = GetOpenConnector(element, null);

            return null != openConnector
              ? openConnector.Owner
              : null;
        }

        /// <summary>
        /// Get the open connector of the system if the system has no base equipment
        /// </summary>
        /// <param name="element">An element in the system</param>
        /// <param name="inputConnector">The connector of the previous element 
        /// to which the element is connected </param>
        /// <returns>The found open connector</returns>
        private Connector GetOpenConnector(Element element, Connector inputConnector)
        {
            Connector openConnector = null;
            ConnectorManager cm = null;
            //
            // Get the connector manager of the element
            if (element is FamilyInstance)
            {
                FamilyInstance fi = element as FamilyInstance;
                cm = fi.MEPModel.ConnectorManager;
            }
            else
            {
                MEPCurve mepCurve = element as MEPCurve;
                cm = mepCurve.ConnectorManager;
            }

            foreach (Connector conn in cm.Connectors)
            {
                // Ignore the connector does not belong to any MEP System or belongs to another different MEP system
                if (conn.MEPSystem == null || !conn.MEPSystem.Id.IntegerValue.Equals(m_system.Id.IntegerValue))
                {
                    continue;
                }

                // If the connector is connected to the input connector, they will have opposite flow directions.
                if (inputConnector != null && conn.IsConnectedTo(inputConnector))
                {
                    continue;
                }

                // If the connector is not connected, it is the open connector
                if (!conn.IsConnected)
                {
                    openConnector = conn;
                    break;
                }

                //
                // If open connector not found, then look for it from elements connected to the element
                foreach (Connector refConnector in conn.AllRefs)
                {
                    // Ignore non-EndConn connectors and connectors of the current element
                    if (refConnector.ConnectorType != ConnectorType.End ||
                        refConnector.Owner.Id.IntegerValue.Equals(conn.Owner.Id.IntegerValue))
                    {
                        continue;
                    }

                    // Ignore connectors of the previous element
                    if (inputConnector != null && refConnector.Owner.Id.IntegerValue.Equals(inputConnector.Owner.Id.IntegerValue))
                    {
                        continue;
                    }

                    openConnector = GetOpenConnector(refConnector.Owner, conn);
                    if (openConnector != null)
                    {
                        return openConnector;
                    }
                }
            }

            return openConnector;
        }

        /// <summary>
        /// Traverse the system recursively by analyzing each element
        /// </summary>
        /// <param name="elementNode">The element to be analyzed</param>
        private void Traverse(TreeNode elementNode)
        {
            int id = elementNode.Id.IntegerValue;

            // Terminate if we revisit a node we have already inspected:

            if (_visitedElementCount.ContainsKey(id))
            {
                return;
            }

            // Otherwise, add the new node to the collection of visited elements:

            if (!_visitedElementCount.ContainsKey(id))
            {
                _visitedElementCount.Add(id, 0);
            }
            ++_visitedElementCount[id];

            //
            // Find all child nodes and analyze them recursively
            AppendChildren(elementNode);
            foreach (TreeNode node in elementNode.ChildNodes)
            {
                Traverse(node);
            }
        }

        /// <summary>
        /// Find all child nodes of the specified element node
        /// </summary>
        /// <param name="elementNode">The specified element node to be analyzed</param>
        private void AppendChildren(TreeNode elementNode)
        {
            List<TreeNode> nodes = elementNode.ChildNodes;
            ConnectorSet connectors;

            // Get connector manager

            Element element = GetElementById(elementNode.Id);

            //Debug.Print( element.Id.IntegerValue.ToString() );

            FamilyInstance fi = element as FamilyInstance;
            if (fi != null)
            {
                connectors = fi.MEPModel.ConnectorManager.Connectors;
            }
            else
            {
                MEPCurve mepCurve = element as MEPCurve;
                connectors = mepCurve.ConnectorManager.Connectors;
            }

            // Find connected connector for each connector
            foreach (Connector connector in connectors)
            {
                MEPSystem mepSystem = connector.MEPSystem;
                // Ignore the connector does not belong to any MEP System or belongs to another different MEP system
                if (mepSystem == null || !mepSystem.Id.IntegerValue.Equals(m_system.Id.IntegerValue))
                {
                    continue;
                }

                //
                // Get the direction of the TreeNode object
                if (elementNode.Parent == null)
                {
                    if (connector.IsConnected)
                    {
                        elementNode.Direction = connector.Direction;
                    }
                }
                else
                {
                    // If the connector is connected to the input connector, they will have opposite flow directions.
                    // Then skip it.
                    if (connector.IsConnectedTo(elementNode.InputConnector))
                    {
                        elementNode.Direction = connector.Direction;
                        continue;
                    }
                }

                // Get the connector connected to current connector
                Connector connectedConnector = GetConnectedConnector(connector);
                if (connectedConnector != null)
                {
                    TreeNode node = new TreeNode(m_document, connectedConnector.Owner.Id);
                    node.InputConnector = connector;
                    node.Parent = elementNode;
                    nodes.Add(node);
                }
            }

            nodes.Sort(delegate (TreeNode t1, TreeNode t2)
            {
                return t1.Id.IntegerValue > t2.Id.IntegerValue ? 1 : (t1.Id.IntegerValue < t2.Id.IntegerValue ? -1 : 0);
            }
            );
        }

        /// <summary>
        /// Get the connected connector of one connector
        /// </summary>
        /// <param name="connector">The connector to be analyzed</param>
        /// <returns>The connected connector</returns>
        static private Connector GetConnectedConnector(Connector connector)
        {
            Connector connectedConnector = null;
            ConnectorSet allRefs = connector.AllRefs;
            foreach (Connector conn in allRefs)
            {
                // Ignore non-EndConn connectors and connectors of the current element
                if (conn.ConnectorType != ConnectorType.End ||
                    conn.Owner.Id.IntegerValue.Equals(connector.Owner.Id.IntegerValue))
                {
                    continue;
                }

                connectedConnector = conn;
                break;
            }

            return connectedConnector;
        }

        /// <summary>
        /// Get element by its id
        /// </summary>
        private Element GetElementById(ElementId eid)
        {
            return m_document.GetElement(eid);
        }
        #endregion // Tree Construction

        #region JSON Output
        /// <summary>
        /// Dump the top-down traversal graph into JSON.
        /// In this case, each parent node is populated
        /// with a full hierarchical graph of all its
        /// children, cf. https://www.jstree.com/docs/json.
        /// </summary>
        public string DumpToJsonTopDown()
        {
            return m_startingElementNode
              .DumpToJsonTopDown();
        }

        /// <summary>
        /// Dump the bottom-up traversal graph into JSON.
        /// In this case, each child node is equipped with 
        /// a 'parent' pointer, cf.
        /// https://www.jstree.com/docs/json/
        /// </summary>
        public string DumpToJsonBottomUp()
        {
            List<string> a = new List<string>();
            m_startingElementNode.DumpToJsonBottomUp(a, "#");
            return "[" + string.Join(",", a) + "]";
        }
        #endregion // JSON Output

        #region XML Output
        /// <summary>
        /// Dump the traversal into an XML file
        /// </summary>
        /// <param name="fileName">Name of the XML file</param>
        public void DumpIntoXML(String fileName)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";
            XmlWriter writer = XmlWriter.Create(fileName, settings);

            // Write the root element
            String mepSystemType = String.Empty;
            mepSystemType = (m_system is MechanicalSystem ? "MechanicalSystem" : "PipingSystem");
            writer.WriteStartElement(mepSystemType);

            // Write basic information of the MEP system
            WriteBasicInfo(writer);
            // Write paths of the traversal
            WritePaths(writer);

            // Close the root element
            writer.WriteEndElement();

            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Write basic information of the MEP system into the XML file
        /// </summary>
        /// <param name="writer">XMLWriter object</param>
        private void WriteBasicInfo(XmlWriter writer)
        {
            MechanicalSystem ms = null;
            PipingSystem ps = null;
            if (m_isMechanicalSystem)
            {
                ms = m_system as MechanicalSystem;
            }
            else
            {
                ps = m_system as PipingSystem;
            }

            // Write basic information of the system
            writer.WriteStartElement("BasicInformation");

            // Write Name property
            writer.WriteStartElement("Name");
            writer.WriteString(m_system.Name);
            writer.WriteEndElement();

            // Write Id property
            writer.WriteStartElement("Id");
            writer.WriteValue(m_system.Id.IntegerValue);
            writer.WriteEndElement();

            // Write UniqueId property
            writer.WriteStartElement("UniqueId");
            writer.WriteString(m_system.UniqueId);
            writer.WriteEndElement();

            // Write SystemType property
            writer.WriteStartElement("SystemType");
            if (m_isMechanicalSystem)
            {
                writer.WriteString(ms.SystemType.ToString());
            }
            else
            {
                writer.WriteString(ps.SystemType.ToString());
            }
            writer.WriteEndElement();

            // Write Category property
            writer.WriteStartElement("Category");
            writer.WriteAttributeString("Id", m_system.Category.Id.IntegerValue.ToString());
            writer.WriteAttributeString("Name", m_system.Category.Name);
            writer.WriteEndElement();

            // Write IsWellConnected property
            writer.WriteStartElement("IsWellConnected");
            if (m_isMechanicalSystem)
            {
                writer.WriteValue(ms.IsWellConnected);
            }
            else
            {
                writer.WriteValue(ps.IsWellConnected);
            }
            writer.WriteEndElement();

            // Write HasBaseEquipment property
            writer.WriteStartElement("HasBaseEquipment");
            bool hasBaseEquipment = ((m_system.BaseEquipment == null) ? false : true);
            writer.WriteValue(hasBaseEquipment);
            writer.WriteEndElement();

            // Write TerminalElementsCount property
            writer.WriteStartElement("TerminalElementsCount");
            writer.WriteValue(m_system.Elements.Size);
            writer.WriteEndElement();

            // Write Flow property
            //writer.WriteStartElement( "Flow" );
            //if( m_isMechanicalSystem )
            //{
            //  writer.WriteValue( ms.GetFlow() );
            //}
            //else
            //{
            //  writer.WriteValue( ps.GetFlow() );
            //}
            //writer.WriteEndElement();

            // Close basic information
            writer.WriteEndElement();
        }

        /// <summary>
        /// Write paths of the traversal into the XML file
        /// </summary>
        /// <param name="writer">XMLWriter object</param>
        private void WritePaths(XmlWriter writer)
        {
            writer.WriteStartElement("Path");
            m_startingElementNode.DumpIntoXML(writer);
            writer.WriteEndElement();
        }

        public void CollectUniqueIds(StringBuilder[] sbs)
        {
            StringBuilder conn_sb = new StringBuilder();

            if (this.m_startingElementNode != null)
            {
                conn_sb.Append("{ \"id\":\"" + TreeNode.GetId(m_system)
                  + "\",\"name\":\"" + TreeNode.GetName(m_system)
                  + "\", \"children\":[], \"udids\":[");

                this.m_startingElementNode.CollectUniqueIds(conn_sb);

                if (conn_sb[conn_sb.Length - 1] == ',')
                {
                    conn_sb.Remove(conn_sb.Length - 1, 1);
                }
                conn_sb.Append("]},");

                string str = conn_sb.ToString();

                if (this.m_system is MechanicalSystem)
                {
                    sbs[0].Append(str);
                }
                if (this.m_system is ElectricalSystem)
                {

                    sbs[1].Append(str);
                }
                if (this.m_system is PipingSystem)
                {
                    sbs[2].Append(str);
                }
            }
        }
        #endregion // XML Output
    }
}
