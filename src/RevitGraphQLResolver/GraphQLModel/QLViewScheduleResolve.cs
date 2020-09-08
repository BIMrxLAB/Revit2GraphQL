using Autodesk.Revit.DB;
using GraphQL.Language.AST;
using RevitGraphQLSchema.GraphQLModel;
using System;
using System.Collections.Generic;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLViewScheduleResolve : QLViewSchedule
    {
        public QLViewScheduleResolve()
        {

        }

        public QLViewScheduleResolve(ViewSchedule aViewSchedule, Field queryFieldForViewScheduleData)
        {
            id = aViewSchedule.Id.ToString();
            name = aViewSchedule.Name;

            if (queryFieldForViewScheduleData != null)
            {
                try
                {
                    qlViewScheduleData = ResolverEntry.aRevitTask.Run<QLViewScheduleData>(app =>
                    {
                        ////https://thebuildingcoder.typepad.com/blog/2012/05/the-schedule-api-and-access-to-schedule-data.html
                        TableData table = aViewSchedule.GetTableData();
                        var rowCount = aViewSchedule.GetTableData().GetSectionData(SectionType.Body).NumberOfRows;
                        var colCount = aViewSchedule.GetTableData().GetSectionData(SectionType.Body).NumberOfColumns;

                        QLViewScheduleData data = new QLViewScheduleData();
                        data.headers = new List<string>();
                        data.rows = new List<QLViewScheduleRow>();

                        for (int i = 0; i < rowCount; i++)
                        {
                            QLViewScheduleRow newRow = new QLViewScheduleRow();
                            newRow.id = i.ToString();
                            newRow.cells = new List<string>();
                            for (int j = 0; j < colCount; j++)
                            {
                                var cellString = aViewSchedule.GetCellText(SectionType.Body, i, j);
                                if (i == 0)
                                {
                                    data.headers.Add(cellString);
                                }
                                else
                                {
                                    newRow.cells.Add(cellString);
                                }
                            }
                            if (i == 0)
                            {
                                i++; // skip 2nd row - it's always blank
                            }
                            else
                            {
                                data.rows.Add(newRow);
                            }
                        }
                        return data;
                    }).Result;
                }
                catch (Exception ex)
                {
                    var m = ex.Message;
                }
            }
        }
    }
}
