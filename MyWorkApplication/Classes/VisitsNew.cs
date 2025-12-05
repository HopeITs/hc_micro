using MySql.Data.MySqlClient;
using MyWorkApplication;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.Linq;

public class VisitsNew
{
    // -------- General Kobo metadata --------
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public DateTime? Today { get; set; }
    public string Username { get; set; }

    // -------- Common project fields --------
    public string Donor { get; set; }
    public string ProjectNumber { get; set; }
    public string BeneficiaryName { get; set; }
    public string Profession { get; set; }

    public string Address { get; set; }
    public string AddressDetails { get; set; }
    public string Region { get; set; }     // NEW
    public string Village { get; set; }    // NEW
    public string Competition { get; set; } // NEW (vehicles)

    // -------- Visit info --------
    public string VisitType { get; set; }  // NEW (vist_, __010, etc.)
    public int? VisitNumber { get; set; }

    // -------- System / problems --------
    public string HasSystemProblems { get; set; }
    public string SystemProblems { get; set; }

    // -------- Development / evaluation --------
    public string HasDevelopment { get; set; }
    public string DevelopmentDetails { get; set; }

    public string Evaluation { get; set; }   // NEW (project_evaluation)
    public string DebtStatus { get; set; }   // NEW (Debt)
    public decimal? DebtAmount { get; set; } // NEW (amount)
    public string Challenges { get; set; }   // NEW (challenges*)

    // -------- Companions --------
    public string Companions { get; set; }
    public string HasOtherTeam { get; set; }
    public string CompanionNames { get; set; }

    // -------- Extra costs --------
    public string HasExtraCosts { get; set; }
    public string ExtraCosts { get; set; }

    public string ExtraCostsEnergy { get; set; }
    public string ExtraCostsRent { get; set; }
    public string ExtraCostsTransport { get; set; }
    public string ExtraCostsOther { get; set; }

    public decimal? ExtraCostAmount { get; set; }
    public string ExtraCostOtherDetails { get; set; }

    // -------- Income --------
    public decimal? AverageIncome { get; set; }
    public string HasChange { get; set; }
    public string ChangeReason { get; set; }

    // -------- Notes --------
    public string TeamNotes { get; set; }
    public string BeneficiaryNotes { get; set; }
    public string Why { get; set; }          // optional reason field in some forms

    // -------- Kobo metadata --------
    public long KoboId { get; set; }
    public string Uuid { get; set; }
    public DateTime? SubmissionTime { get; set; }
    public string ValidationStatus { get; set; }
    public string Notes { get; set; }
    public string Status { get; set; }
    public string SubmittedBy { get; set; }
    public string Version { get; set; }
    public string Tags { get; set; }
    public int? Index { get; set; }

    // ========================================
    // DB METHODS
    // ========================================

    public void InsertVisit(VisitsNew v)
    {
        Program.buildConnection();

        string sql = @"
INSERT INTO VisitsNew
(start, end, today, username, donor, project_number, beneficiary_name,
 profession, address, address_details, companions, has_other_team,
 companion_names, visit_number, has_system_problems, system_problems,
 has_development, development_details, has_extra_costs, extra_costs,
 extra_costs_energy, extra_costs_rent, extra_costs_transport, extra_costs_other,
 extra_cost_amount, extra_cost_other_details, average_income, has_change,
 change_reason, team_notes, beneficiary_notes, kobo_id, uuid, submission_time,
 validation_status, notes, status, submitted_by, version, tags, form_index)
VALUES
(@start,@end,@today,@username,@donor,@project_number,@beneficiary_name,
 @profession,@address,@address_details,@companions,@has_other_team,
 @companion_names,@visit_number,@has_system_problems,@system_problems,
 @has_development,@development_details,@has_extra_costs,@extra_costs,
 @extra_costs_energy,@extra_costs_rent,@extra_costs_transport,@extra_costs_other,
 @extra_cost_amount,@extra_cost_other_details,@average_income,@has_change,
 @change_reason,@team_notes,@beneficiary_notes,@kobo_id,@uuid,@submission_time,
 @validation_status,@notes,@status,@submitted_by,@version,@tags,@index)";

        using (var cmd = new MySqlCommand(sql, Program.MyConn))
        {
            cmd.Parameters.AddWithValue("@start", v.Start);
            cmd.Parameters.AddWithValue("@end", v.End);
            cmd.Parameters.AddWithValue("@today", v.Today);
            cmd.Parameters.AddWithValue("@username", v.Username);

            cmd.Parameters.AddWithValue("@donor", v.Donor);
            cmd.Parameters.AddWithValue("@project_number", v.ProjectNumber);
            cmd.Parameters.AddWithValue("@beneficiary_name", v.BeneficiaryName);
            cmd.Parameters.AddWithValue("@profession", v.Profession);
            cmd.Parameters.AddWithValue("@address", v.Address);
            cmd.Parameters.AddWithValue("@address_details", v.AddressDetails);

            cmd.Parameters.AddWithValue("@companions", v.Companions);
            cmd.Parameters.AddWithValue("@has_other_team", v.HasOtherTeam);
            cmd.Parameters.AddWithValue("@companion_names", v.CompanionNames);

            cmd.Parameters.AddWithValue("@visit_number", v.VisitNumber);
            cmd.Parameters.AddWithValue("@has_system_problems", v.HasSystemProblems);
            cmd.Parameters.AddWithValue("@system_problems", v.SystemProblems);

            cmd.Parameters.AddWithValue("@has_development", v.HasDevelopment);
            cmd.Parameters.AddWithValue("@development_details", v.DevelopmentDetails);

            cmd.Parameters.AddWithValue("@has_extra_costs", v.HasExtraCosts);
            cmd.Parameters.AddWithValue("@extra_costs", v.ExtraCosts);

            cmd.Parameters.AddWithValue("@extra_costs_energy", v.ExtraCostsEnergy);
            cmd.Parameters.AddWithValue("@extra_costs_rent", v.ExtraCostsRent);
            cmd.Parameters.AddWithValue("@extra_costs_transport", v.ExtraCostsTransport);
            cmd.Parameters.AddWithValue("@extra_costs_other", v.ExtraCostsOther);

            cmd.Parameters.AddWithValue("@extra_cost_amount", v.ExtraCostAmount);
            cmd.Parameters.AddWithValue("@extra_cost_other_details", v.ExtraCostOtherDetails);

            cmd.Parameters.AddWithValue("@average_income", v.AverageIncome);
            cmd.Parameters.AddWithValue("@has_change", v.HasChange);
            cmd.Parameters.AddWithValue("@change_reason", v.ChangeReason);

            cmd.Parameters.AddWithValue("@team_notes", v.TeamNotes);
            cmd.Parameters.AddWithValue("@beneficiary_notes", v.BeneficiaryNotes);

            cmd.Parameters.AddWithValue("@kobo_id", v.KoboId);
            cmd.Parameters.AddWithValue("@uuid", v.Uuid);
            cmd.Parameters.AddWithValue("@submission_time", v.SubmissionTime);
            cmd.Parameters.AddWithValue("@validation_status", v.ValidationStatus);
            cmd.Parameters.AddWithValue("@notes", v.Notes);
            cmd.Parameters.AddWithValue("@status", v.Status);
            cmd.Parameters.AddWithValue("@submitted_by", v.SubmittedBy);
            cmd.Parameters.AddWithValue("@version", v.Version);
            cmd.Parameters.AddWithValue("@tags", v.Tags);
            cmd.Parameters.AddWithValue("@index", v.Index);

            cmd.ExecuteNonQuery();
        }

        Program.MyConn.Close();
    }

    public bool Exists(long koboId)
    {
        Program.buildConnection();

        string sql = "SELECT COUNT(*) FROM VisitsNew WHERE kobo_id=@id";

        using (var cmd = new MySqlCommand(sql, Program.MyConn))
        {
            cmd.Parameters.AddWithValue("@id", koboId);
            var value = Convert.ToInt32(cmd.ExecuteScalar());
            Program.MyConn.Close();
            return value > 0;
        }
    }

    // ========================================
    // VALIDATION
    // ========================================
    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public List<string> Errors { get; set; } = new List<string>();
    }

    public ValidationResult ValidateSubmission(JObject s)
    {
        var v = new ValidationResult();

        if (string.IsNullOrWhiteSpace((string)s["group_mo5jr41/bene_name"]))
            v.Errors.Add("اسم المستفيد: مطلوب");

        if (!DateTime.TryParse((string)s["today"], out _))
            v.Errors.Add("التاريخ اليوم غير صالح");

        // Optional: add more per-form rules here

        return v;
    }

    // ========================================
    // Small helper conversion methods
    // ========================================
    public static DateTime? ToDate(JToken token)
    {
        DateTime d;
        return token != null && DateTime.TryParse(token.ToString(), out d)
            ? (DateTime?)d
            : null;
    }

    public static decimal? ToDecimal(JToken token)
    {
        decimal d;
        return token != null && decimal.TryParse(token.ToString(), out d)
            ? (decimal?)d
            : null;
    }

    public static int? ToInt(JToken token)
    {
        int i;
        return token != null && int.TryParse(token.ToString(), out i)
            ? (int?)i
            : null;
    }

    public static long ToLong(JToken token)
    {
        long l;
        return token != null && long.TryParse(token.ToString(), out l)
            ? l
            : 0;
    }
}
