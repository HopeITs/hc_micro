using System;
using System.Collections.Generic;
using System.Data;

namespace MyWorkApplication.Classes
{
    public static class KoboMonitoringIdentity
    {
        public static DataRow ResolveParticipant(DataTable relationships)
        {
            Dictionary<int, DataRow> beneficiaries = new Dictionary<int, DataRow>();
            Dictionary<int, DataRow> partners = new Dictionary<int, DataRow>();
            foreach (DataRow row in relationships.Rows)
            {
                string role = row["PersonType"].ToString().Trim();
                if (row["Person_ID"] == DBNull.Value) continue;
                int id = Convert.ToInt32(row["Person_ID"]);
                if (id <= 0) continue;
                if (role == "مستفيد") beneficiaries[id] = row;
                else if (role == "شريك") partners[id] = row;
            }
            Dictionary<int, DataRow> candidates = beneficiaries.Count > 0 ? beneficiaries : partners;
            if (candidates.Count != 1)
                throw new InvalidOperationException("Project must identify exactly one beneficiary, or exactly one partner when no beneficiary exists. Found " + candidates.Count + ". No person was guessed.");
            foreach (DataRow row in candidates.Values) return row;
            throw new InvalidOperationException("No verified project participant.");
        }

        public static bool RequiresProjectIsolation(DataTable projects, int sourceProject)
        {
            HashSet<int> ids = new HashSet<int>();
            foreach (DataRow row in projects.Rows)
                if (row["MicroProject_ID"] != DBNull.Value) ids.Add(Convert.ToInt32(row["MicroProject_ID"]));
            if (!ids.Contains(sourceProject))
                throw new InvalidOperationException("Person is not linked to the explicit Kobo project.");
            return ids.Count > 1;
        }
    }
}
