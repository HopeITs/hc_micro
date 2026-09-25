namespace MyWorkApplication.Classes
{
    internal static class StatisticsReportQuery
    {
        // Count both series over the same filtered join once. The previous implementation
        // ran another large joined count for every category returned by the outer query.
        internal static string Build(string dimension, string label, string group, string from,
            string condition, bool beneficiaries = false, string orderBy = null)
        {
            string id = beneficiaries ? "P.P_ID" : "MP.MP_ID";
            string distinct = beneficiaries ? "" : "DISTINCT ";
            return "SELECT " + dimension + " AS `" + label + "`, "
                + "COUNT(" + distinct + id + ") AS numOfProjects, "
                + "COUNT(" + distinct + "CASE WHEN mps.State_ID IN (4,5,7,8) THEN " + id + " END) AS numOfFundedProjects, "
                + group + " AS ReportGroupId " + from + condition
                + " GROUP BY " + group + ", " + dimension
                + " ORDER BY " + (orderBy ?? group) + ";";
        }
    }
}
