using System;

namespace MyWorkApplication.Classes
{
    public static class KoboSyncUserMessages
    {
        public static string Conflict(string visit, string existing, string incoming, string state)
        {
            if(state=="resolved") return "تم حسم هذا التعارض. يمكنك الاطلاع على القرار واسم المستخدم وتاريخه من سجل القرارات.";
            if(state=="stale") return "تغيرت بيانات إحدى الاستمارتين بعد المراجعة السابقة. قارن البيانات الحالية مجدداً قبل اعتماد القرار؛ لم يُنفذ استبدال تلقائي.";
            if(state=="blocked") return "تعذر تطبيق القرار بسبب تغير ارتباط الزيارة أو وجود تعديلات تحتاج فحصاً. لم يُنفذ استبدال تلقائي؛ يرجى مراجعة مسؤول النظام.";
            if(state=="approved") return "تم حفظ القرار، لكن تطبيقه لم يكتمل. لم يُعتمد الاستبدال تلقائياً؛ يرجى مراجعة مسؤول النظام لفحص سبب التعذر.";
            if(state=="deferred") return "تم تأجيل القرار بشأن الاستمارة " + incoming + ". لم تُضف زيارة أخرى ولم تُستبدل بيانات الزيارة الحالية.";
            return "وردت استمارتان مختلفتان لنفس المشروع والشخص ومرحلة الزيارة وتاريخها. الزيارة الحالية " + visit +
                " مرتبطة بالاستمارة " + existing + "، والاستمارة الواردة " + incoming +
                " قد تكون تصحيحاً أو زيارة أخرى. قارن البيانات واختر القرار المناسب؛ لم تُستبدل البيانات تلقائياً.";
        }

        public static string Summary(int errors,int conflicts)
        {
            errors=Math.Max(0,errors); conflicts=Math.Max(0,Math.Min(errors,conflicts));
            string message="انتهت عملية المزامنة، لكن " + errors + " حالة تحتاج إلى متابعة.";
            if(conflicts>0)
                message+="\r\n\r\nتعارضات بين زيارات متشابهة: " + conflicts +
                    ". أوقف النظام اعتماد البيانات الواردة لهذه الحالات حتى لا يستبدل زيارة أو يكررها دون تأكيدك.\r\nافتح «مراجعة التعارضات» لمقارنة البيانات واختيار القرار المناسب.";
            int other=errors-conflicts;
            if(other>0)
                message+="\r\n\r\nحالات أخرى لم تكتمل: " + other +
                    ". لحالات اسم المستفيد غير المطابق، افتح «مراجعة التعارضات» ثم «مطابقة المستفيدين». أما أخطاء المشروع أو الاتصال فتحتاج مراجعة مسؤول النظام؛ لا يمكن حل جميع الحالات باختيار شخص.";
            return message;
        }
    }
}
