using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkApplication.Classes
{
    public static class KoboMappers
    {
        public static VisitsNew Map_EnergyProjects_Aleppo(JObject s)
        {
            return new VisitsNew
            {
                Donor = (string)s["group_mo5jr41/Doner"],
                ProjectNumber = (string)s["group_mo5jr41/text_fw08u96"],
                BeneficiaryName = (string)s["group_mo5jr41/bene_name"],
                Profession = (string)s["group_mo5jr41/Career"],
                Address = (string)s["group_mo5jr41/text_qp0vk54"],
                Region = (string)s["group_mo5jr41/select_one_cd5qq26"],
                Village = (string)s["group_mo5jr41/select_one_vt8tw79"],
                VisitType = (string)s["vist_"],

                HasSystemProblems = (string)s["group_le9la00/working_fine"],
                HasDevelopment = (string)s["group_le9la00/project_improvment"],
                DevelopmentDetails = (string)s["group_le9la00/what_is_improvment"],

                HasExtraCosts = (string)s["group_le9la00/additional_fees"],
                AverageIncome = (decimal?)(s["group_le9la00/income"]),
                HasChange = (string)s["group_le9la00/income_change"],
                ChangeReason = (string)s["group_le9la00/change_reson"],

                KoboId = Convert.ToInt64(s["_id"]),
                Uuid = (string)s["_uuid"],
                SubmissionTime = (DateTime?)(s["_submission_time"]),
            };
        }


        // TODO: Add the other form mappings here:
        public static VisitsNew Map_ME2025_Aleppo(JObject s)
        {
            return new VisitsNew
            {
                Donor = (string)s["group_mo5jr41/Doner"],
                ProjectNumber = (string)s["group_mo5jr41/text_fw08u96"],
                BeneficiaryName = (string)s["group_mo5jr41/bene_name"],
                Profession = (string)s["group_mo5jr41/career"],
                Region = (string)s["group_mo5jr41/select_one_cd5qq26"],
                Address = (string)s["group_mo5jr41/text_qp0vk54"],
                Village = (string)s["group_mo5jr41/select_one_vt8tw79"],

                VisitType = (string)s["__010"],
                VisitNumber = (int?)(s["integer_pi0rg69"]),

                AverageIncome = (decimal?)(s["income"]),
                ChangeReason = (string)s["text_iv7zp41"],
                TeamNotes = (string)s["text_oo9jk98"],
                BeneficiaryNotes = (string)s["text_ro1pp04"],

                KoboId = Convert.ToInt64(s["_id"]),
                SubmissionTime = (DateTime?)(s["_submission_time"]),
            };
        }

        public static VisitsNew Map_VehiclesOpening_Aleppo(JObject s) {
            return new VisitsNew
            {
                Donor = (string)s["group_mo5jr41/Doner"],
                ProjectNumber = (string)s["group_mo5jr41/__003"],
                BeneficiaryName = (string)s["group_mo5jr41/bene_name"],
                Profession = (string)s["group_mo5jr41/career"],

                Region = (string)s["group_mo5jr41/__004"],
                Address = (string)s["group_mo5jr41/__005"],
                Competition = (string)s["group_mo5jr41/__006"],

                VisitType = (string)s["group_na4gu14/vist_no"],
                Evaluation = (string)s["group_na4gu14/project_evaluation"],
                HasDevelopment = (string)s["group_na4gu14/paying"],
                DebtStatus = (string)s["group_na4gu14/Debt"],
                DebtAmount = (decimal?)(s["group_na4gu14/amount"]),

                AverageIncome = null,

                KoboId = Convert.ToInt64(s["_id"]),
                SubmissionTime = (DateTime?)(s["_submission_time"]),
            };
        }
        public static VisitsNew Map_VehiclesMonitoring_Aleppo(JObject s)
        {
            return new VisitsNew
            {
                Donor = (string)s["group_mo5jr41/Doner"],
                ProjectNumber = (string)s["group_mo5jr41/__003"],
                BeneficiaryName = (string)s["group_mo5jr41/bene_name"],
                Profession = (string)s["group_mo5jr41/career"],

                Region = (string)s["group_mo5jr41/__004"],
                Address = (string)s["group_mo5jr41/__005"],
                Competition = (string)s["group_mo5jr41/__006"],

                VisitType = (string)s["group_na4gu14/vist_no"],
                HasChange = (string)s["group_na4gu14/income_change"],
                ChangeReason = (string)s["group_na4gu14/reason_income_why"],
                Challenges = (string)s["group_na4gu14/challenges"],
                DevelopmentDetails = (string)s["group_na4gu14/change_made"],

                KoboId = Convert.ToInt64(s["_id"]),
                SubmissionTime = (DateTime?)(s["_submission_time"]),
            };
        }
        public static VisitsNew Map_FinalVisit_Aleppo(JObject s) {
            return new VisitsNew
            {
                Donor = (string)s["group_mo5jr41/Doner"],
                ProjectNumber = (string)s["group_mo5jr41/text_fw08u96"],
                BeneficiaryName = (string)s["group_mo5jr41/bene_name"],
                Profession = (string)s["group_mo5jr41/Career"],
                Region = (string)s["group_mo5jr41/select_one_cd5qq26"],
                Address = (string)s["group_mo5jr41/text_qp0vk54"],

                HasDevelopment = (string)s["group_tl5ny44/project_target"],
                DevelopmentDetails = (string)s["group_tl5ny44/PT_Why"],
                AverageIncome = (decimal?)(s["group_tl5ny44/income"]),
                HasChange = (string)s["group_tl5ny44/income_change"],

                Challenges = (string)s["group_tl5ny44/challenges_reson"],
                HasExtraCosts = (string)s["group_tl5ny44/Xtra_fees"],
                ExtraCosts = (string)s["group_tl5ny44/fees_typ"],
                ExtraCostAmount = (decimal?)(s["group_tl5ny44/feez_amount"]),

                KoboId = Convert.ToInt64(s["_id"]),
                SubmissionTime = (DateTime?)(s["_submission_time"]),
            };
        }
        public static VisitsNew Map_OpeningVisit2025_Aleppo(JObject s) {
            return new VisitsNew
            {
                Donor = (string)s["group_mo5jr41/Doner"],
                ProjectNumber = (string)s["group_mo5jr41/text_fw08u96"],
                BeneficiaryName = (string)s["group_mo5jr41/bene_name"],
                Profession = (string)s["group_mo5jr41/Career"],
                Address = (string)s["group_mo5jr41/text_qp0vk54"],

                VisitNumber = (int?)(s["group_wk1ua11/integer_fy1gq00"]),
                CompanionNames = (string)s["group_wk1ua11/text_xg9hv27"],
                HasDevelopment = (string)s["group_wk1ua11/implementing"],
                ExtraCostAmount = (decimal?)(s["group_wk1ua11/integer_zb84p56"]),
                ExtraCostOtherDetails = (string)s["group_wk1ua11/extra_money"],
                AverageIncome = (decimal?)(s["group_wk1ua11/integer_yx5il31"]),

                KoboId = Convert.ToInt64(s["_id"]),
                SubmissionTime = (DateTime?)(s["_submission_time"]),
            };
        }
    }
}
