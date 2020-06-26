namespace TraverseAllSystems
{
    class Options
    {
        /// <summary>
        /// Store element id or UniqueId in JSON output?
        /// </summary>
        public static bool StoreUniqueId = true;
        public static bool StoreElementId = !StoreUniqueId;

        /// <summary>
        /// Store parent node id in child, or recursive
        /// tree of children in parent?
        /// </summary>
        public static bool StoreJsonGraphBottomUp = false;
        public static bool StoreJsonGraphTopDown
          = !StoreJsonGraphBottomUp;

        /// <summary>
        /// Store entire JSON graph for all systems on
        /// project info element, or individual graph for
        /// each system seaparately in MEP system element?
        /// </summary>
        public static bool StoreEntireJsonGraphOnProjectInfo = true;
        public static bool StoreSeparateJsonGraphOnEachSystem
          = !StoreEntireJsonGraphOnProjectInfo;

        /// <summary>
        /// The JSON tag specifying the tree node label, 
        /// normally either 'name' or 'text'.
        /// </summary>
        public static string NodeLabelTag = "text";
    }
}