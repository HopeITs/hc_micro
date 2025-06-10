DROP PROCEDURE IF EXISTS `beneficiaries_me_answers`;
DROP PROCEDURE IF EXISTS `calculate_profit_rates`;
DROP PROCEDURE IF EXISTS `empty_database`;
DROP PROCEDURE IF EXISTS `mevisit_results`;
DROP PROCEDURE IF EXISTS `select_application_details`;
DROP PROCEDURE IF EXISTS `select_checklist_details`;
DROP PROCEDURE IF EXISTS `select_loan_details`;
DROP PROCEDURE IF EXISTS `visit_user_statistics`;
 
DELIMITER $$
CREATE  PROCEDURE `beneficiaries_me_answers`(IN `MyCondition` TEXT ,IN `MyType` VARCHAR(255))
    NO SQL
BEGIN
SET GLOBAL group_concat_max_len = 1000000;

SET @sql = NULL;

SELECT GROUP_CONCAT(DISTINCT CONCAT('max(CASE WHEN mevisit_answer.Question_ID = ''',mequestion.ID,''' then meanswer.Name END) AS `',mequestion.Name,'`')) INTO @sql
 FROM mequestion 
 WHERE RIGHT(mequestion.Type,1) LIKE MyType;
 
SET @sql = CONCAT('SELECT person_microproject.MicroProject_ID as ''رقم المشروع'' 
, CONCAT(P_FirstName, '' '', P_LastName, '' ابن/ة '', P_FatherName) as ''المستفيد''   
, (SELECT Name from subcategory where subcategory.ID = microproject.SubCategory_ID) as ''Sub Category''  
, (SELECT Name from career where career.ID = microproject.Career_ID) as ''Career'' 
, (SELECT Name_abb from donor where donor.ID = microproject.MP_Donor) as ''الممول''   
, mevisit.Number as ''رقم الزيارة'' 
, mevisit.Profit as ''الربح الصافي'' 
, ' 
, @sql
, ' FROM microproject 
 LEFT JOIN person_microproject on microproject.MP_ID = person_microproject.MicroProject_ID
 LEFT JOIN person ON person.P_ID = person_microproject.Person_ID
 LEFT JOIN mevisit ON mevisit.Person_ID = person.P_ID
 LEFT JOIN mevisit_answer ON mevisit_answer.Visit_ID = mevisit.ID
 LEFT JOIN meanswer on meanswer.ID = mevisit_answer.Answer_ID
 LEFT JOIN mequestion on mequestion.ID = mevisit_answer.Question_ID  
 WHERE mevisit.kind =''',MyType,'''
 GROUP BY  mevisit_answer.Visit_ID  
 ORDER BY `person_microproject`.`MicroProject_ID` ASC, mevisit.Number ASC;');

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;


-- SELECT @sql;

END$$
DELIMITER ;

DELIMITER $$
CREATE  PROCEDURE `calculate_profit_rates`(IN `MyCondition` TEXT)
BEGIN 
 DROP TABLE IF EXISTS t1;
 CREATE TABLE t1 
 SELECT mevisit.Person_ID as 'Person_ID'
 
 ,(SELECT IFNULL(avg(W_Salary),0) FROM `person_workexp` WHERE `W_Status` like 'Yes' and `Person_ID` =  mevisit.Person_ID) as 'AvgSal'
 
 ,(SELECT IFNULL(avg(me.`Profit`),0) FROM `mevisit` me WHERE me.`Person_ID` = mevisit.Person_ID) as 'AvgEval' 
 , (case WHEN   
	(
		(SELECT IFNULL(avg(W_Salary),0) FROM `person_workexp` WHERE `W_Status` like 'Yes' and `Person_ID` =  mevisit.Person_ID) = 0 
		AND 
		(SELECT IFNULL(avg(me.`Profit`),0) FROM `mevisit` me WHERE me.`Person_ID` = mevisit.Person_ID) = 0 
	)
	THEN '-1000000000' 
	WHEN   
	(
		(SELECT IFNULL(avg(W_Salary),0) FROM `person_workexp` WHERE `W_Status` like 'Yes' and `Person_ID` =  mevisit.Person_ID) = 0 
		AND 
		(SELECT IFNULL(avg(me.`Profit`),0) FROM `mevisit` me WHERE me.`Person_ID` = mevisit.Person_ID) <> 0
	)
	THEN '100' 
	ELSE 
		(	
			(
			 (SELECT IFNULL(avg(me.`Profit`),0) FROM `mevisit` me WHERE me.`Person_ID` = mevisit.Person_ID)
			 - 
			 (SELECT IFNULL(avg(W_Salary),0) FROM `person_workexp` WHERE `W_Status` like 'Yes' and `Person_ID` =  mevisit.Person_ID)
			)
			/
			(SELECT IFNULL(avg(W_Salary),0) FROM `person_workexp` WHERE `W_Status` like 'Yes' and `Person_ID` =  mevisit.Person_ID)
		)*100 
	End )as 'EvalSal_Percentage'
                    
 FROM mevisit
 left join `person_microproject` PMP on mevisit.Person_ID = PMP.Person_ID  
 WHERE PMP.PersonType like 'مستفيد'					
 GROUP BY mevisit.Person_ID;
 
 SET @sql1 = 'SELECT CASE \r\n when t1.EvalSal_Percentage BETWEEN -999999999 and -1 then ''<0 %'' \r\n when t1.EvalSal_Percentage = 0 then ''0 %'' \r\n when t1.EvalSal_Percentage BETWEEN 1 and 20 then ''1-20 %'' \r\n when t1.EvalSal_Percentage BETWEEN 20 and 40 then ''21-40 %''  \r\n when t1.EvalSal_Percentage BETWEEN 40 and 60 then ''41-60 %'' \r\n when t1.EvalSal_Percentage BETWEEN 60 and 80 then ''61-80 %''  \r\n when t1.EvalSal_Percentage BETWEEN 80 and 99 then ''81-99 %''  \r\n when t1.EvalSal_Percentage = 100 then ''100 %''  \r\n when t1.EvalSal_Percentage > 100 then ''>100 %''  \r\n when t1.EvalSal_Percentage = -1000000000 then ''N/A''   \r\n else ''bubu'' End as ''MyRate''   \r\n \r\n ,count(DISTINCT t1.Person_ID) as ''numOfProjects''  \r\n\r\nFROM t1 \r\n\tleft join `person_microproject` PMP  on t1.Person_ID = PMP.Person_ID\r\n\tleft join `microproject` MP  on PMP.MicroProject_ID = MP.MP_ID\r\n\tleft join `person` P on PMP.Person_ID = P.P_ID\r\n\tleft join `loan` L on MP.MP_ID = L.MicroProject_ID \r\n\tleft join `category` C on MP.Category_ID = C.ID\r\n\tLEFT join `subcategory` sub on sub.ID =  MP.SubCategory_ID \r\n\tLEFT join `state` on MP.MP_State = state.ID \r\n\tLeft join `donor` on MP.MP_Donor = donor.ID\r\n\tLEFT join `donorgroup` on donorgroup.ID = MP.DonorGroup_ID  \r\n\tLEFT join `fundtype` on fundtype.ID = MP.MP_FundType_ID \r\n\tLEFT join `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID\r\n\tLEFT join `microprojectsubtype` on microprojectsubtype.ID = MP.MP_SubType_ID\r\n';

 SET @where1 = ' ';
 SET @groupBy1 = ' group by MyRate ORDER BY t1.`EvalSal_Percentage` ';
  
 IF (MyCondition <> '') THEN 
 SET @where1 = concat(@where1,' ',MyCondition); 
 End IF; 
 
 SET @sql1 = concat(@sql1,' ',@where1,' ',@groupBy1);

 PREPARE stmt FROM @sql1;
 EXECUTE stmt;  
 DROP TABLE IF EXISTS t1;
 
END$$
DELIMITER ;

 DELIMITER $$
 CREATE  PROCEDURE `empty_database`()
BEGIN
DELETE from checklist_member;
DELETE FROM donorgroup;
DELETE from exefile;
DELETE from family;
DELETE from file;
DELETE from image;
DELETE from loan;
DELETE from log;
DELETE from material;
DELETE from mevisit;
DELETE from mevisit_answer;
DELETE from mevisit_user;
DELETE from microproject;
DELETE from microproject_score;
DELETE from microproject_risk;
DELETE from office_monitoring;
DELETE from payment;
DELETE from person;
DELETE from person_course;
DELETE from person_education;
DELETE from person_family;
DELETE from person_loss;
DELETE from person_microproject;
DELETE from person_workexp;
DELETE from priest;
DELETE from task_microproject;
DELETE from task_user;
DELETE from user_notification;
DELETE from visit;
DELETE from visit_user; 
END$$
DELIMITER ;

DELIMITER $$
CREATE  PROCEDURE `mevisit_results`(IN `MyCondition` TEXT, IN `MyType` TEXT)
BEGIN

SET @inner_condition1 = ' ';
IF (MyCondition <> '') THEN 
SET @inner_condition1 = concat(@inner_condition1,' ',MyCondition); End IF; 
SET @where1 = ' where 1 ';

SET @sql1 = concat(',(SELECT count(*) from `mevisit_answer` left join `mevisit` on mevisit.ID = mevisit_answer.Visit_ID  where mevisit_answer.Question_ID = mequestion.ID and mevisit_answer.Answer_ID = ans1_ID '
			,@inner_condition1
			,') as ''ans1_count'' ');
SET @sql2 = concat(',(SELECT count(*) from `mevisit_answer`  left join `mevisit` on mevisit.ID = mevisit_answer.Visit_ID  where mevisit_answer.Question_ID = mequestion.ID and mevisit_answer.Answer_ID = ans2_ID '
			,@inner_condition1
			,') as ''ans2_count'' ');
SET @sql3 = concat(',(SELECT count(*) from `mevisit_answer`  left join `mevisit` on mevisit.ID = mevisit_answer.Visit_ID  where mevisit_answer.Question_ID = mequestion.ID and mevisit_answer.Answer_ID = ans3_ID '
			,@inner_condition1
			,') as ''ans3_count'' ');

SET @sql4 = concat(',(SELECT count(*) from `mevisit_answer`  left join `mevisit` on mevisit.ID = mevisit_answer.Visit_ID  where mevisit_answer.Question_ID = mequestion.ID '
			,@inner_condition1
			,') as ''all_ans_count'' ');			
                
SET @sql_all = concat('SELECT mequestion.ID , Name , (SELECT Name from meanswer WHERE meanswer.Question_ID = mequestion.ID ORDER by ID ASC LIMIT 1) as ''ans1''  , IFNULL( (SELECT Name from meanswer WHERE meanswer.Question_ID = mequestion.ID     and meanswer.ID >  (SELECT ID from meanswer WHERE meanswer.Question_ID = mequestion.ID ORDER by ID ASC LIMIT 1)     and meanswer.ID <  (SELECT ID from meanswer WHERE meanswer.Question_ID = mequestion.ID ORDER by ID DESC LIMIT 1)      LIMIT 1) ,'''') as ''ans2''  , (SELECT Name from meanswer WHERE meanswer.Question_ID = mequestion.ID ORDER by ID DESC LIMIT 1) as ''ans3''  , (SELECT ID from meanswer WHERE meanswer.Question_ID = mequestion.ID ORDER by ID ASC LIMIT 1) as ''ans1_ID''  , IFNULL( (SELECT ID from meanswer WHERE meanswer.Question_ID = mequestion.ID    and meanswer.ID > (SELECT ID from meanswer WHERE meanswer.Question_ID = mequestion.ID ORDER by ID ASC LIMIT 1)    and meanswer.ID < (SELECT ID from meanswer WHERE meanswer.Question_ID = mequestion.ID ORDER by ID DESC LIMIT 1)      LIMIT 1) ,'''' ) as ''ans2_ID''  , (SELECT ID from meanswer WHERE meanswer.Question_ID = mequestion.ID ORDER by ID DESC LIMIT 1) as ''ans3_ID'''

,@sql1
,@sql2
,@sql3
,@sql4
,' FROM `mequestion` ');

IF (MyType <> '') THEN 
SET @where1 = concat(@where1,' and mequestion.`Type` like ''%',MyType,'%'''); End IF; 
SET @orderBy1 = ' order by ID ';

set @sql_all = concat(@sql_all,@where1,@orderBy1,';');

SET @ben1 = '';

set @query = concat(@sql_all, @ben1);

PREPARE stmt FROM @query;
EXECUTE stmt;  

END$$
DELIMITER ;

DELIMITER $$
CREATE  PROCEDURE `select_application_details`(IN `MyCondition` TEXT CHARSET utf8mb4)
    NO SQL
BEGIN
 
 SET @sql1 = '  select microproject.MP_ID as ''رقم المشروع''  \r\n , case `Partnership` when 1 then ''فردي'' else ''شراكة'' end as ''حالة الشراكة'' \r\n , fundtype.Name as ''نوع التمويل'' \r\n 
 , microprojecttype.Name as ''نوع المشروع'' \r\n 
 , microprojectsubtype.Name as ''تفصيل نوع المشروع'' \r\n 
 , category.Name as ''الصنف'' \r\n 
 , subcategory.Name as ''المهنة''  \r\n 
 , `P_PicturePath` as ''الصورة الشخصية''   \r\n 
 , `MP_Message` as ''رسالة الرفض''  \r\n , `MP_Visited` as ''زيارة المنزل''   \r\n , `MP_VisitDate` as ''تاريخ الزيارة'' \r\n , `MP_VisitTime` as ''توقيت الزيارة''  \r\n , `MP_VisitNotes` as ''ملاحظات عن الزيارة'' \r\n , `IsContentUpdated` as ''هل تم تبديل المشروع'' \r\n , `MP_Name` as ''اسم المشروع''\r\n , `MP_RequestedAmount` as ''المبلغ المطلوب'' \r\n , `MP_DateOfRequest` as ''تاريخ تعبئة الطلب'' \r\n , `MP_PeriodOfExecution` as ''مدة تنفيذ المشروع''  \r\n 
 
 , person.P_ID as ''رقم المستفيد'' \r\n 
 , concat(`P_FirstName`, '' '', `P_LastName`) as ''اسم المستفيد'' \r\n 
 , `P_FatherName` as ''اسم الأب'' \r\n 
 , `P_MotherName` as ''اسم الأم'' \r\n , `P_Sex` as ''الجنس''   \r\n , maritalstatus.Name as ''الحالة الاجتماعية''  \r\n , personstate.Name as ''الحالة'' \r\n , `P_NationalNumber` as ''الرقم الوطني'' \r\n , year(`P_DOB`) as ''المواليد''\r\n , `P_MilitaryService` as ''خدمة العلم'' \r\n , `P_MilitaryService_Note` as ''السبب'' \r\n , `P_IntermidiarySide` as ''الهيئة الوسيطة'' \r\n , `P_Parish` as ''الطائفة'' \r\n , priest.Priest_Name as ''الكاهن المحلي''  \r\n , `P_Mobile` as ''رقم الجوال'' , `P_HomeTel` as ''رقم الأرضي'' \r\n , `P_RegistrationPlace` as ''الأمانة'' , street.Name as ''منطقة المنزل'' \r\n , `P_HomeAddress` as ''العنوان''  , `P_IsLivingWithFamily` as ''تسكن مع عائلة أخرى'' \r\n , `P_LiveWithFamily_Note` as ''من هم'' \r\n , `P_HomeState` as ''مكان السكن''\r\n , `P_HomeState_Note` as ''ملاحظة عن مكان السكن'' \r\n , case `P_OtherProperties` when 1 then ''سيارة وملك مؤجر'' when 2 then ''سيارة'' when 3 then ''ملك مؤجر'' else ''N/A'' end as ''أملاك أخرى''   \r\n , `P_OtherProperties_Note` as ''ملاحظة عن أملاك أخرى''    \r\n , `P_NumAtHome` as ''عدد أفراد المنزل''  , family.F_Number as ''رقم دفتر العائلة''   \r\n \r\n , (select GROUP_CONCAT(CONCAT_WS(''-'', person.P_FirstName,person.P_FatherName,person.P_LastName, year(person.P_DOB) \r\n\t,person_family.Relation, person_family.Work_Name) )    \r\n\tFROM person_family     \r\n\tLEFT JOIN person p on p.P_ID = person_family.Person_ID     \r\n\tWHERE person_family.Person_ID = person.P_ID         \r\n\tAND person_family.Relation not LIKE ''مستفيد'') as ''أفراد العائلة''   \r\n\t\r\n, (select GROUP_CONCAT(CONCAT_WS(''-'', work.W_Name , person_workexp.W_Description, person_workexp.W_BeginDate,person_workexp.W_EndDate , person_workexp.W_Place , person_workexp.W_PlaceState , person_workexp.W_Salary , person_workexp.W_CauseOfLose, person_workexp.W_CauseOfLoseDescription ))     FROM work     LEFT JOIN person_workexp on work.W_ID = person_workexp.Work_ID     WHERE person_workexp.Person_ID = person.P_ID       and person_workexp.W_Status = ''Yes'') as ''العمل الحالي''       \r\n\r\n, (select GROUP_CONCAT(CONCAT_WS(''-'', work.W_Name , person_workexp.W_Description, person_workexp.W_BeginDate,person_workexp.W_EndDate , person_workexp.W_Place , person_workexp.W_PlaceState , person_workexp.W_Salary , person_workexp.W_CauseOfLose, person_workexp.W_CauseOfLoseDescription ))     FROM work     LEFT JOIN person_workexp on work.W_ID = person_workexp.Work_ID     WHERE person_workexp.Person_ID = person.P_ID       and person_workexp.W_Status = ''No'') as ''العمل السابق''  \r\n\r\n, (select GROUP_CONCAT(concat(education.E_Type,''-'',education.E_Name))     FROM education     LEFT JOIN person_education on education.E_ID = person_education.Education_ID     WHERE person_education.Person_ID = person.P_ID ) as ''التحصيل العلمي''      \r\n\r\n, (select GROUP_CONCAT(course.Name)     FROM course     LEFT JOIN person_course on course.ID = person_course.Course_ID     WHERE person_course.Person_ID = person.P_ID  ) as ''الدورات المتبعة''  \r\n\r\n, `P_Courses_Note` as ''ملاحظات عن الدورات''\r\n, `IsProvider` as ''هل أنت المعيل الوحيد'' \r\n, `P_SourceOfIncome` as ''المعيلين في المنزل'' \r\n, case `P_OtherIncomeSources` when 1 then ''أقارب ومعونة''   when 2 then ''أقارب'' when 2 then ''معونة'' else ''N/A'' end as ''المصادر المساعدة'' \r\n, `P_OtherIncomeSources_Note` as ''مصادر مساعدة اخرى''  \r\n, `P_MedicalHelp` as ''هل يوجد مصادر طبية'', `P_MedicalHelp_Note` as ''المصادر الطبية''  \r\n, `P_MedicalCond` as ''حالة مرضية''    \r\n\r\n, (select GROUP_CONCAT(loss.Name)     FROM loss     LEFT JOIN person_loss on loss.ID = person_loss.Loss_ID     WHERE person_loss.Person_ID = person.P_ID  ) as ''خسارات'' \r\n\r\n, `P_Loss` as ''ملاحظة عن الخسارات''    \r\n\r\n, `MP_Description` as ''وصف المشروع'' \r\n, `MP_HasPreviousDonors` as ''استفادة من برامج سابقة''\r\n, `MP_OtherDonors` as ''الجهات السابقة''   \r\n, (select GROUP_CONCAT(CONCAT_WS(''-'', score.Name, microproject_score.value, microproject_score.notes))    FROM score     LEFT JOIN microproject_score on score.ID = microproject_score.Score_ID    WHERE microproject_score.MicroProject_ID = microproject.MP_ID ) as ''تحليل المشروع''        \r\n\r\n, `MP_IsNeedLicense` as ''هل يحتاج تراخيص'', `MP_LicenseSide` as ''الترخيص'' \r\n, `MP_HasCompetitors` as ''هل يوجد منافسة'', `MP_Competitors` as ''المنافسون'' \r\n, `MP_SuppliesInsurance` as ''تأمين اللوازم'', `MP_Suppliers` as ''الموردون''  \r\n\r\n , (select GROUP_CONCAT(risk.Name)    FROM risk     LEFT JOIN microproject_risk on risk.ID = microproject_risk.Risk_ID    WHERE microproject_risk.MicroProject_ID = microproject.MP_ID ) as ''المخاطر'' \r\n \r\n , `MP_Risk` as ''ملاحظات عن المخاطر'' , `MP_Protection` as ''آلية الوقاية''  \r\n , `MP_OtherNotes` as ''معلومات إضافية'', `MP_Marketing` as ''استراتيجية التسويق'' \r\n , `MP_SimpleProfit` as ''الربح الصافي'', `MP_Profit_Description` as ''ملاحظات عن الربح''  \r\n , case `MP_IncomeKind` when ''Single'' then ''Yes'' else ''No'' end as ''هل المشروع هو الدخل الوحيد'' \r\n \r\n , `MP_IncomeKind_Note` as ''ملاحظات عن الدخل'' , `MP_HasLedger` as ''هل تلتزم بدفتر حسابات''   , (select GROUP_CONCAT(CONCAT_WS(''-'', material.Name, material.Amount, material.Price , material.LocalContribution, material.Comments))    \r\n FROM material     WHERE material.MicroProject_ID = microproject.MP_ID ) as ''الميزانية''     \r\n , (SELECT user.UserName FROM user WHERE user.UserID = MP_PaperUser_ID LIMIT 1 ) as ''مسجل الاستمارة'' , (SELECT user.UserName FROM user WHERE user.UserID = MP_ProgramUser_ID LIMIT 1 ) as ''مدخل الاستمارة'' , (SELECT user.UserName FROM user WHERE user.UserID = MP_EditedByUser_ID LIMIT 1 ) as ''المعدّل على الاستمارة''   , state.Name_ar as ''حالة المشروع'', `MP_StateDate` as ''تاريخ تبديل الحالة''    , donor.Name as ''الممول''  , donorgroup.Name ''المجموعة''     \r\n\r\n from `microproject`  \r\n LEFT outer join person_microproject on person_microproject.MicroProject_ID = microproject.MP_ID  \r\n LEFT outer join person on person.P_ID = person_microproject.Person_ID    \r\n LEFT outer join person_course on person_course.Person_ID = person.P_ID  \r\n LEFT outer join person_family on person_family.Person_ID = person.P_ID   \r\n LEFT OUTER JOIN family on family.F_ID = person_family.Family_ID  \r\n LEFT OUTER JOIN priest on priest.Priest_ID = person.P_Priest_ID  \r\n LEFT OUTER JOIN street on street.ID = person.Street_ID  \r\n LEFT outer join personstate on personstate.Name  = person.P_State_ID   \r\n LEFT OUTER JOIN maritalstatus on person.P_MaritalStatus_ID = maritalstatus.ID \r\n LEFT outer join category on microproject.Category_ID = category.ID    \r\n LEFT outer join subcategory on microproject.SubCategory_ID = subcategory.ID    \r\n LEFT outer join microprojecttype on microproject.MP_Type_ID = microprojecttype.ID    \r\n LEFT outer join microprojectsubtype on microproject.MP_SubType_ID = microprojectsubtype.ID     \r\n LEFT outer join fundtype on microproject.MP_FundType_ID = fundtype.ID   \r\n LEFT outer join state on microproject.MP_State  = state.ID    \r\n LEFT outer join donor on microproject.MP_Donor = donor.ID    \r\n LEFT OUTER JOIN donorgroup on microproject.DonorGroup_ID = donorgroup.ID ';
 
 SET @where1 = ''; 
 IF (MyCondition <> '') THEN 
 	SET @where1 = concat(@where1,' ',MyCondition);
 End If;
 
 set @groupby1 = ' GROUP BY person.P_ID ';
 set @orderby1 = ' ORDER BY  microproject.MP_ID DESC ';
  
 set @query = concat(@sql1,@where1,@groupby1,@orderby1);

PREPARE stmt FROM @query;
EXECUTE stmt;   

END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE `select_checklist_details`(IN `MyCondition` TEXT)
    NO SQL
BEGIN
 
 SET @sql1 = '  SELECT MP_ID as ''رقم المشروع'' 
, IF(MP_ParishNotes = '''',''N/A'', MP_ParishNotes) as ''الطائفة''
, IF(MP_KeyPersonNotes = '''',''N/A'', MP_KeyPersonNotes) as ''ملاحظات أخرى''
 
,IFNULL((SELECT GROUP_CONCAT(concat(checklist_member.Name, '' - '' , 
(CASE microproject_checklist.Opinion WHEN 0 THEN ''ضد'' when 1 THEN ''مع'' ELSE ''N/A'' END) 
	, IF(microproject_checklist.Notes = '''','''','' - '' ) 
	, REPLACE(REPLACE(microproject_checklist.Notes, ''\r\n'', '' ''), ''\n'', '' '') )
	SEPARATOR ''\r\n'')
  FROM `microproject_checklist` 
  left join checklist_member on microproject_checklist.Member_ID = checklist_member.ID 
  WHERE microproject_checklist.MicroProject_ID = MP.MP_ID AND Location like ''k''),''N/A'') as ''رأي المفتاحيين''	  

,(CASE MP.ch_HouseTechCondition 
  WHEN 0 THEN ''دون'' 
  when 1 THEN ''عادي'' 
  when 2 THEN ''جيد''  
  ELSE ''N/A'' END ) as ''الحالة الفنية''
  
 ,(CASE MP.ch_ProjectExperience
  WHEN 0 THEN ''لا يوجد'' 
  when 1 THEN ''قليلة'' 
  when 2 THEN ''وسط''  
  when 3 THEN ''ممتازة''  
  ELSE ''N/A'' END ) as ''الخبرة في المشروع''

,(CASE MP.ch_ExpectedProjectSuccess 
  WHEN 0 THEN ''نعم'' 
  when 1 THEN ''لا''   
  ELSE ''N/A'' END ) as ''تعديل على الميزانية''

,(CASE MP.ch_WorkAbility 
  WHEN 0 THEN ''غير قادر'' 
  when 1 THEN ''قادر''  
  ELSE ''N/A'' END ) as ''القدرة الصحية على العمل''
  
,IF(MP_VisitNotes = '''',''N/A'', MP_VisitNotes) as ''ملاحظات الزيارة''
   
, IFNULL((SELECT GROUP_CONCAT(
	concat(checklist_member.Name
	, IF(microproject_checklist.Notes = '''','''','' - '' ) 
	, REPLACE(REPLACE(microproject_checklist.Notes, ''\r\n'', '' ''), ''\n'', '' '') ) )
	FROM `microproject_checklist`  
    left join checklist_member on microproject_checklist.Member_ID = checklist_member.ID 
	WHERE microproject_checklist.MicroProject_ID = MP.MP_ID AND Location like ''v'') , ''N/A'') as ''الزائرين'' 
   
 ,IF(MP.MP_VisitTime = '''',''N/A'' ,MP.MP_VisitTime) as ''التوقيت''
 ,IFNULL(MP.MP_VisitDate,''N/A'') as ''التاريخ''
     
,IFNULL((SELECT GROUP_CONCAT(concat(checklist_member.Name, '' - '' , 
(CASE microproject_checklist.Opinion WHEN 0 THEN ''ضد'' when 1 THEN ''مع'' ELSE ''N/A'' END) 
	, IF(microproject_checklist.Notes = '''','''','' - '' ) 
	, REPLACE(REPLACE(microproject_checklist.Notes, ''\r\n'', '' ''), ''\n'', '' '') )
	SEPARATOR ''\r\n'')
  FROM `microproject_checklist` 
  left join checklist_member on microproject_checklist.Member_ID = checklist_member.ID 
  WHERE microproject_checklist.MicroProject_ID = MP.MP_ID AND Location like ''t''),''N/A'') as ''رأي الفريق''	 
     
,IFNULL(MP.MP_TeamDate,''N/A'') as ''تاريخ اجتماع الفريق''

 ,IFNULL((SELECT GROUP_CONCAT(concat(checklist_member.Name, '' - '' , 
(CASE microproject_checklist.Opinion WHEN 0 THEN ''ضد'' when 1 THEN ''مع'' ELSE ''N/A'' END) 
	, IF(microproject_checklist.Notes = '''','''','' - '' ) 
	, REPLACE(REPLACE(microproject_checklist.Notes, ''\r\n'', '' ''), ''\n'', '' '') )
	SEPARATOR ''\r\n'')
  FROM `microproject_checklist` 
  left join checklist_member on microproject_checklist.Member_ID = checklist_member.ID 
  WHERE microproject_checklist.MicroProject_ID = MP.MP_ID AND Location like ''c''),''N/A'') as ''رأي اللجنة''	 
   
, IFNULL(MP.MP_CommityDate,''N/A'') as ''تاريخ اجتماع اللجنة''
   
, IFNULL(state.Name_ar,''N/A'') as ''القرار النهائي''
, IFNULL(MP.MP_StateDate,''N/A'') as ''تاريخ القرار النهائي''
, IFNULL(CONCAT(IF(statereason.Name = ''-'',''N/A'',statereason.Name)
	, IF(MP_StateReason = '''','''','' - '' )
	, MP_StateReason),''N/A'') as ''السبب''
   
 FROM microproject MP 
 left join `state` on MP.MP_State = state.ID 
 left join `statereason` on MP.MP_StateReason_ID = statereason.ID  ';
 
 SET @where1 = ''; 
 IF (MyCondition <> '') THEN 
 	SET @where1 = concat(@where1,' ',MyCondition);
 End If;
 
 set @groupby1 = ' ';
 set @orderby1 = ' ';
  
 set @query = concat(@sql1,@where1,@groupby1,@orderby1);

PREPARE stmt FROM @query;
EXECUTE stmt;   

END$$
DELIMITER ;

DELIMITER $$
CREATE  PROCEDURE `select_loan_details`()
BEGIN
select L.Loan_ID as 'ID'
,L.MicroProject_ID as 'رقم المشروع' 
,CONCAT(P.P_FirstName, ' ',P.P_FatherName, ' ', P.P_LastName) as 'المستفيد'
,L.Loan_DateTaken as 'تاريخ استلام القرض'
,L.Loan_Amount as 'القرض'
,Round( ( Loan_Amount / (SELECT Rate from donorgroup where donorgroup.ID = MP.DonorGroup_ID)) , 2 ) as 'القرض ($)'
,L.Loan_Rate as 'النسبة' 
,L.Loan_ReturnedAmount as 'المبلغ المطلوب استرداده'
,L.Loan_PaymentsAmount as 'القسط'
,L.Loan_PaymentsCount as 'عدد الدفعات الكلي'
,(select count(Pay_Amount) from `payment` where `Loan_ID` = L.Loan_ID and `Pay_IsPaid` like N'Paid') as 'عدد الدفعات المدفوعة'
,((IF(((L.Loan_Amount * L.Loan_Rate / 100) - (select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID))<0 
    ,0,((L.Loan_Amount * L.Loan_Rate / 100) - (select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID))) )
    / L.Loan_PaymentsAmount ) as 'عدد الدفعات المتبقية'
,(select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID) as 'المبلغ المدفوع'
,IF(((L.Loan_Amount * L.Loan_Rate / 100) - (select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID))<0
    ,0,((L.Loan_Amount * L.Loan_Rate / 100) - (select sum(`Pay_Amount`) from `payment` where `Pay_IsPaid` like N'Paid' and `Loan_ID` = L.Loan_ID))) as 'المبلغ المتبقي'

,(SELECT `Pay_DueDate` FROM `payment` WHERE `Loan_ID` = L.Loan_ID and `Pay_IsPaid` like 'Paid' ORDER BY `payment`.`Pay_DueDate` DESC LIMIT 1) as 'تاريخ آخر دفعة مدفوعة'
,(SELECT `Pay_DueDate` FROM `payment` WHERE `Loan_ID` = L.Loan_ID and `Pay_IsPaid` like 'Not Paid'  AND state.Name_ar like 'ممول' ORDER BY `payment`.`Pay_DueDate` DESC LIMIT 1) as 'تاريخ الدفعة المستحقة'
,(SELECT TIMESTAMPDIFF(MONTH, payment.Pay_DueDate , DATE_ADD( Now(),INTERVAL 1 MONTH)) from payment where payment.Loan_ID = L.Loan_ID and payment.Pay_IsPaid = 'Not Paid' AND state.Name_ar like 'ممول' ORDER by Pay_DueDate desc LIMIT 1 ) as 'عدد أشهر التأخير'

,donorgroup.Name as 'المجموعة'
,L.Loan_ReceiptID as 'رقم الإيصال'

from `loan` L 
 left outer join `microproject` MP on L.MicroProject_ID = MP.MP_ID 
 LEFT outer join person_microproject PMP on PMP.MicroProject_ID = MP.MP_ID 
 LEFT outer join person P on P.P_ID = PMP.Person_ID 
 LEFT outer join `category` C on MP.Category_ID = C.ID 
 LEFT outer join `state` on state.ID = MP.MP_State 
 LEFT outer join `donorgroup` on donorgroup.ID = MP.DonorGroup_ID;
 
 END$$
DELIMITER ;

DELIMITER $$
CREATE  PROCEDURE `visit_user_statistics`(IN `FromDate` TINYTEXT, IN `ToDate` TINYTEXT, IN `MP_Type` TINYTEXT)
BEGIN

 SET @q1 = '';
 SET @q2 = '';
 SET @q3 = '';
 SET @where1 = '';
 
IF (MP_Type <> '') THEN 
SET @where1 = concat(@where1, ' and microprojecttype.Name like ''%' ,MP_Type , '%'' '); End IF;  
  
set @q1 = concat(
 ' SELECT UserName as ''الزائر'',
	(SELECT COUNT(v.ID) FROM visit v LEFT JOIN visit_user vu ON vu.Visit_ID = v.ID  
 WHERE v.Kind like ''%op'' and vu.User_ID = visit_user.User_ID
 AND v.Date between ''',FromDate,''' AND ''',ToDate,''' ) as ''Opening'',
	(SELECT COUNT(me.ID) FROM mevisit me  
 LEFT JOIN mevisit_user meu ON meu.Visit_ID = me.ID  WHERE me.Number=''1'' and meu.User_ID = mevisit_user.User_ID
 AND me.Date between ''',FromDate,''' AND ''',ToDate,''' ) as ''M1'',
     (SELECT COUNT(me.ID) FROM mevisit me  
 LEFT JOIN mevisit_user meu ON meu.Visit_ID = me.ID  WHERE me.Number=''2'' and meu.User_ID = mevisit_user.User_ID AND me.Date between ''',FromDate,''' AND ''',ToDate,''') as ''M2'',
     (SELECT COUNT(me.ID) FROM mevisit me  
 LEFT JOIN mevisit_user meu ON meu.Visit_ID = me.ID  WHERE me.Number=''3'' and meu.User_ID = mevisit_user.User_ID AND me.Date between ''',FromDate,''' AND ''',ToDate,''') as ''M3'',
     (SELECT COUNT(me.ID) FROM mevisit me  
 LEFT JOIN mevisit_user meu ON meu.Visit_ID = me.ID  WHERE me.Number=''4'' and meu.User_ID = mevisit_user.User_ID AND me.Date between ''',FromDate,''' AND ''',ToDate,''') as ''M4'',
     (SELECT COUNT(me.ID) FROM mevisit me  
 LEFT JOIN mevisit_user meu ON meu.Visit_ID = me.ID  WHERE me.Number=''5'' and meu.User_ID = mevisit_user.User_ID AND me.Date between ''',FromDate,''' AND ''',ToDate,''') as ''M5'',
	(SELECT COUNT(v.ID) FROM visit v LEFT JOIN visit_user vu ON vu.Visit_ID = v.ID
 WHERE v.Kind like ''%cl'' and vu.User_ID = visit_user.User_ID
 AND v.Date between ''',FromDate,''' AND ''',ToDate,''' ) as ''Closing'',
   
   (SELECT COUNT(v.ID) FROM visit v LEFT JOIN visit_user vu ON vu.Visit_ID = v.ID  
 WHERE  vu.User_ID = visit_user.User_ID AND v.Date between ''',FromDate,''' AND ''',ToDate,''') 
 +  
     (SELECT COUNT(me.ID) FROM mevisit me LEFT JOIN mevisit_user meu ON meu.Visit_ID = me.ID  WHERE meu.User_ID = mevisit_user.User_ID AND me.Date between ''',FromDate,''' AND ''',ToDate,''') 
 as ''مجموع الزيارات''
  
 From `user`  
 LEFT JOIN `visit_user` ON visit_user.User_ID = user.UserID 
 LEFT JOIN `visit` ON visit_user.Visit_ID = visit.ID
 LEFT JOIN `mevisit_user` ON mevisit_user.User_ID = user.UserID  
 LEFT join `mevisit` on mevisit_user.Visit_ID = mevisit.ID
 LEFT join `microproject` on microproject.MP_ID = visit.MicroProject_ID
 LEFT join `microprojecttype` on microprojecttype.ID = microproject.MP_Type_ID 
 
 WHERE user.IsVisitor = 1 
 AND (visit.Date between ''',FromDate,''' AND ''',ToDate,'''
 OR mevisit.Date between ''',FromDate,''' AND ''',ToDate,''') ' , @where1 ,  
 ' GROUP BY `الزائر`  
   ORDER BY `مجموع الزيارات` DESC ', ';' );
  
  set @q2 = concat(
    ' SELECT   
	  IFNULL( SUM(IF(visit.Kind like ''%op'', 1, 0)),0) as ''sum op''
    , IFNULL( SUM(IF(visit.Kind like ''%cl'', 1, 0)),0) as ''sum cl'' 
    , IFNULL(COUNT(visit.ID),0) as ''total visit''  
    from `visit`
	LEFT join `microproject` MP on visit.MicroProject_ID = MP.MP_ID
    LEFT join `category` C on MP.Category_ID = C.ID 
    LEFT join `state` on state.ID = MP.MP_State  
    LEFT join `donor` on donor.ID = MP.MP_Donor 
    LEFT join `loan` L on L.MicroProject_ID = MP.MP_ID
    LEFT join `donorgroup` on donorgroup.ID = MP.DonorGroup_ID 
	LEFT join `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID   
	where visit.Date between ''',FromDate,''' AND ''',ToDate,''' ' , @where1 , ';') ;

set @q3 = concat(
	' SELECT
	  IFNULL( SUM(IF(Number like ''1'', 1, 0)),0) as ''sum m1'' 
	 ,IFNULL( SUM(IF(Number like ''2'', 1, 0)),0) as ''sum m2'' 
	 ,IFNULL( SUM(IF(Number like ''3'', 1, 0)),0) as ''sum m3'' 
	 ,IFNULL( SUM(IF(Number like ''4'', 1, 0)),0) as ''sum m4'' 
	 ,IFNULL( SUM(IF(Number like ''5'', 1, 0)),0) as ''sum m5'' 
	 ,IFNULL(COUNT(mevisit.ID), 0) as ''total mevisit''  
 from `microproject` MP
 LEFT join `person_microproject` PMP on MP.MP_ID = PMP.MicroProject_ID 
 LEFT join `mevisit` on PMP.Person_ID = mevisit.Person_ID 
 LEFT join `category` C on MP.Category_ID = C.ID 
 LEFT join `state` on state.ID = MP.MP_State  
 LEFT join `donor` on donor.ID = MP.MP_Donor  
 LEFT join `loan` L on L.MicroProject_ID = MP.MP_ID
 LEFT join `donorgroup` on donorgroup.ID = MP.DonorGroup_ID 
 LEFT join `microprojecttype` on microprojecttype.ID = MP.MP_Type_ID 
 where ( mevisit.Date between ''',FromDate,''' AND ''',ToDate,''' )
 and PMP.PersonType like ''مستفيد'' ' , @where1 , ';');
   
 PREPARE stmt FROM @q1;
 EXECUTE stmt;  
 
 PREPARE stmt FROM @q2;
 EXECUTE stmt;  
 
 PREPARE stmt FROM @q3;
 EXECUTE stmt;  
  
END$$
DELIMITER ;
