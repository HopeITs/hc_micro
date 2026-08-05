-- Profession Meetings database schema
-- MySQL / MariaDB
-- This file adds the tables needed to store:
--   1. استبيان لقاء أصحاب المهن
--   2. محضر اجتماع لقاء أصحاب المهن
--
-- Design notes:
-- - Main meeting data is stored once in profession_meeting.
-- - Fixed sections/questions/options are lookup tables.
-- - Normal answers use profession_meeting_answer and profession_meeting_option_answer.
-- - Repeated grids use dedicated child tables for easier reports/statistics.
-- - Existing app tables referenced here: user.UserID, career.ID, subcategory.ID,
--   person.P_ID, microproject.MP_ID.

SET NAMES utf8;
SET FOREIGN_KEY_CHECKS = 0;

CREATE TABLE IF NOT EXISTS `profession_section` (
  `ID` TINYINT NOT NULL,
  `Name_Ar` VARCHAR(150) NOT NULL,
  `Sort_Order` TINYINT NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `uq_profession_section_sort` (`Sort_Order`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_question` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Section_ID` TINYINT NOT NULL,
  `Number` VARCHAR(5) NOT NULL,
  `Text_Ar` VARCHAR(700) NOT NULL,
  `Answer_Type` VARCHAR(50) NOT NULL,
  `Is_Active` TINYINT(1) NOT NULL DEFAULT 1,
  `Sort_Order` SMALLINT NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `uq_profession_question_number` (`Number`),
  UNIQUE KEY `uq_profession_question_sort` (`Sort_Order`),
  KEY `idx_profession_question_section` (`Section_ID`),
  CONSTRAINT `fk_profession_question_section`
    FOREIGN KEY (`Section_ID`) REFERENCES `profession_section` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_option` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Question_ID` INT NOT NULL,
  `Text_Ar` VARCHAR(200) NOT NULL,
  `Is_Other` TINYINT(1) NOT NULL DEFAULT 0,
  `Has_Details` TINYINT(1) NOT NULL DEFAULT 0,
  `Sort_Order` SMALLINT NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `uq_profession_option_question_text` (`Question_ID`, `Text_Ar`),
  UNIQUE KEY `uq_profession_option_question_sort` (`Question_ID`, `Sort_Order`),
  KEY `idx_profession_option_question` (`Question_ID`),
  CONSTRAINT `fk_profession_option_question`
    FOREIGN KEY (`Question_ID`) REFERENCES `profession_question` (`ID`)
    ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_meeting` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Career_ID` INT NOT NULL,
  `SubCategory_ID` INT NULL,
  `MeetingDate` DATE NOT NULL,
  `Place` VARCHAR(200) NULL,
  `InvitedCount` SMALLINT NOT NULL DEFAULT 0,
  `AttendanceCount` SMALLINT NOT NULL DEFAULT 0,
  `AbsenceCount` SMALLINT NOT NULL DEFAULT 0,
  `PaperRegistrar_UserID` INT NULL,
  `ProgramRegistrar_UserID` INT NOT NULL,
  `Created_By_UserID` INT NOT NULL,
  `Created_At` DATETIME NOT NULL,
  `Edited_By_UserID` INT NULL,
  `Updated_At` DATETIME NULL,
  `ChurchFunders_Note` TEXT NULL,
  PRIMARY KEY (`ID`),
  KEY `idx_profession_meeting_career` (`Career_ID`),
  KEY `idx_profession_meeting_subcategory` (`SubCategory_ID`),
  KEY `idx_profession_meeting_date` (`MeetingDate`),
  KEY `idx_profession_meeting_paper_user` (`PaperRegistrar_UserID`),
  KEY `idx_profession_meeting_program_user` (`ProgramRegistrar_UserID`),
  KEY `idx_profession_meeting_created_user` (`Created_By_UserID`),
  KEY `idx_profession_meeting_edited_user` (`Edited_By_UserID`),
  CONSTRAINT `fk_profession_meeting_career`
    FOREIGN KEY (`Career_ID`) REFERENCES `career` (`ID`),
  CONSTRAINT `fk_profession_meeting_subcategory`
    FOREIGN KEY (`SubCategory_ID`) REFERENCES `subcategory` (`ID`),
  CONSTRAINT `fk_profession_meeting_paper_user`
    FOREIGN KEY (`PaperRegistrar_UserID`) REFERENCES `user` (`UserID`),
  CONSTRAINT `fk_profession_meeting_program_user`
    FOREIGN KEY (`ProgramRegistrar_UserID`) REFERENCES `user` (`UserID`),
  CONSTRAINT `fk_profession_meeting_created_user`
    FOREIGN KEY (`Created_By_UserID`) REFERENCES `user` (`UserID`),
  CONSTRAINT `fk_profession_meeting_edited_user`
    FOREIGN KEY (`Edited_By_UserID`) REFERENCES `user` (`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_meeting_answer` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Meeting_ID` INT NOT NULL,
  `Question_ID` INT NOT NULL,
  `Value_Text` TEXT NULL,
  `Value_Number` INT NULL,
  `Rtf_Content` LONGTEXT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `uq_profession_meeting_answer_question` (`Meeting_ID`, `Question_ID`),
  KEY `idx_profession_meeting_answer_question` (`Question_ID`),
  CONSTRAINT `fk_profession_meeting_answer_meeting`
    FOREIGN KEY (`Meeting_ID`) REFERENCES `profession_meeting` (`ID`)
    ON DELETE CASCADE,
  CONSTRAINT `fk_profession_meeting_answer_question`
    FOREIGN KEY (`Question_ID`) REFERENCES `profession_question` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_meeting_option_answer` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Meeting_ID` INT NOT NULL,
  `Option_ID` INT NOT NULL,
  `Is_Selected` TINYINT(1) NOT NULL DEFAULT 0,
  `Rank_Value` INT NULL,
  `Other_Text` VARCHAR(300) NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `uq_profession_meeting_option_answer` (`Meeting_ID`, `Option_ID`),
  KEY `idx_profession_option_answer_option` (`Option_ID`),
  CONSTRAINT `fk_profession_option_answer_meeting`
    FOREIGN KEY (`Meeting_ID`) REFERENCES `profession_meeting` (`ID`)
    ON DELETE CASCADE,
  CONSTRAINT `fk_profession_option_answer_option`
    FOREIGN KEY (`Option_ID`) REFERENCES `profession_option` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_meeting_competition` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Meeting_ID` INT NOT NULL,
  `Competition_Level` VARCHAR(50) NOT NULL,
  `CountValue` INT NULL,
  `Reason` VARCHAR(500) NULL,
  `Sort_Order` TINYINT NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `uq_profession_competition_level` (`Meeting_ID`, `Competition_Level`),
  CONSTRAINT `fk_profession_competition_meeting`
    FOREIGN KEY (`Meeting_ID`) REFERENCES `profession_meeting` (`ID`)
    ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_meeting_season` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Meeting_ID` INT NOT NULL,
  `From_Period` VARCHAR(50) NULL,
  `To_Period` VARCHAR(50) NULL,
  `Reason` VARCHAR(300) NULL,
  `Sort_Order` TINYINT NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `idx_profession_season_meeting` (`Meeting_ID`),
  CONSTRAINT `fk_profession_season_meeting`
    FOREIGN KEY (`Meeting_ID`) REFERENCES `profession_meeting` (`ID`)
    ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_meeting_brand` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Meeting_ID` INT NOT NULL,
  `Brand_Name` VARCHAR(200) NOT NULL,
  `Sort_Order` TINYINT NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `idx_profession_brand_meeting` (`Meeting_ID`),
  CONSTRAINT `fk_profession_brand_meeting`
    FOREIGN KEY (`Meeting_ID`) REFERENCES `profession_meeting` (`ID`)
    ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_meeting_equipment` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Meeting_ID` INT NOT NULL,
  `Specialization` VARCHAR(150) NULL,
  `Equipment` TEXT NULL,
  `Sort_Order` TINYINT NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `idx_profession_equipment_meeting` (`Meeting_ID`),
  CONSTRAINT `fk_profession_equipment_meeting`
    FOREIGN KEY (`Meeting_ID`) REFERENCES `profession_meeting` (`ID`)
    ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_meeting_challenge` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Meeting_ID` INT NOT NULL,
  `Challenge` VARCHAR(400) NULL,
  `Challenge_Count` INT NULL,
  `Suggested_Solution` VARCHAR(500) NULL,
  `Sort_Order` TINYINT NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `idx_profession_challenge_meeting` (`Meeting_ID`),
  KEY `idx_profession_challenge_text` (`Challenge`(100)),
  CONSTRAINT `fk_profession_challenge_meeting`
    FOREIGN KEY (`Meeting_ID`) REFERENCES `profession_meeting` (`ID`)
    ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_meeting_attendee` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Meeting_ID` INT NOT NULL,
  `Person_ID` INT NULL,
  `MicroProject_ID` INT NULL,
  `Attendee_Name` VARCHAR(150) NULL,
  `Speciality` VARCHAR(150) NULL,
  `Sort_Order` SMALLINT NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `idx_profession_attendee_meeting` (`Meeting_ID`),
  KEY `idx_profession_attendee_person` (`Person_ID`),
  KEY `idx_profession_attendee_project` (`MicroProject_ID`),
  CONSTRAINT `fk_profession_attendee_meeting`
    FOREIGN KEY (`Meeting_ID`) REFERENCES `profession_meeting` (`ID`)
    ON DELETE CASCADE,
  CONSTRAINT `fk_profession_attendee_person`
    FOREIGN KEY (`Person_ID`) REFERENCES `person` (`P_ID`),
  CONSTRAINT `fk_profession_attendee_project`
    FOREIGN KEY (`MicroProject_ID`) REFERENCES `microproject` (`MP_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_meeting_participant` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Meeting_ID` INT NOT NULL,
  `List_Type` VARCHAR(50) NOT NULL,
  `Question_ID` INT NULL,
  `Person_ID` INT NULL,
  `MicroProject_ID` INT NULL,
  `Person_Name` VARCHAR(150) NULL,
  `Speciality` VARCHAR(150) NULL,
  `Notes` VARCHAR(300) NULL,
  `Sort_Order` SMALLINT NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `idx_profession_participant_meeting` (`Meeting_ID`),
  KEY `idx_profession_participant_type` (`List_Type`),
  KEY `idx_profession_participant_question` (`Question_ID`),
  KEY `idx_profession_participant_person` (`Person_ID`),
  KEY `idx_profession_participant_project` (`MicroProject_ID`),
  CONSTRAINT `fk_profession_participant_meeting`
    FOREIGN KEY (`Meeting_ID`) REFERENCES `profession_meeting` (`ID`)
    ON DELETE CASCADE,
  CONSTRAINT `fk_profession_participant_question`
    FOREIGN KEY (`Question_ID`) REFERENCES `profession_question` (`ID`),
  CONSTRAINT `fk_profession_participant_person`
    FOREIGN KEY (`Person_ID`) REFERENCES `person` (`P_ID`),
  CONSTRAINT `fk_profession_participant_project`
    FOREIGN KEY (`MicroProject_ID`) REFERENCES `microproject` (`MP_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

CREATE TABLE IF NOT EXISTS `profession_meeting_minute` (
  `ID` INT NOT NULL AUTO_INCREMENT,
  `Meeting_ID` INT NOT NULL,
  `Section_ID` TINYINT NOT NULL,
  `Notes` TEXT NULL,
  `Rtf_Content` LONGTEXT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `uq_profession_minute_section` (`Meeting_ID`, `Section_ID`),
  KEY `idx_profession_minute_section` (`Section_ID`),
  CONSTRAINT `fk_profession_minute_meeting`
    FOREIGN KEY (`Meeting_ID`) REFERENCES `profession_meeting` (`ID`)
    ON DELETE CASCADE,
  CONSTRAINT `fk_profession_minute_section`
    FOREIGN KEY (`Section_ID`) REFERENCES `profession_section` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

SET FOREIGN_KEY_CHECKS = 1;

-- Seed fixed sections.
INSERT INTO `profession_section` (`ID`, `Name_Ar`, `Sort_Order`) VALUES
  (1, 'المحور الأول: الخبرة والممارسة المهنية', 1),
  (2, 'المحور الثاني: دراسة السوق والبيئة التنافسية', 2),
  (3, 'المحور الثالث: المعدات والتجهيزات والتطورات التقنية', 3),
  (4, 'المحور الرابع: التحديات والمشاكل والحلول المقترحة', 4),
  (5, 'المحور الخامس: التدريب، التطوير والمشاركة المجتمعية', 5)
ON DUPLICATE KEY UPDATE
  `Name_Ar` = VALUES(`Name_Ar`),
  `Sort_Order` = VALUES(`Sort_Order`);

-- Seed fixed questions.
INSERT INTO `profession_question`
  (`Section_ID`, `Number`, `Text_Ar`, `Answer_Type`, `Is_Active`, `Sort_Order`)
VALUES
  (1, '1',  'كيف بدأت تعلم المهنة واكتساب خبرتك؟', 'option_number', 1, 1),
  (1, '2',  'كم عدد السنوات التي قمت فيها بممارسة هذه المهنة؟', 'option_number', 1, 2),
  (1, '3',  'ما هو الوقت المطلوب لتعلم أساسيات هذه المهنة نظريا وعمليا ويصبح المتدرب قادر على فتح مشروع؟', 'option_number', 1, 3),
  (2, '4',  'ماهي الفئة المستهدفة او الزبائن الأساسيين لخدمات ومنتجات هذه المهنة في الوقت الحالي؟', 'multi_option', 1, 4),
  (2, '5',  'ماهي أكثر الخدمات أو المنتجات طلبا من قبل الزبائن في هذه المهنة حاليا؟', 'text', 1, 5),
  (2, '6',  'كيف تقيم مستوى المنافسة في هذه المهنة ضمن منطقتك الجغرافية؟ ولماذا؟', 'competition', 1, 6),
  (2, '7',  'ما هو العامل الأهم والأكثر تأثيرا لجذب الزبائن في هذه المهنة؟', 'option_number', 1, 7),
  (2, '8',  'بناء على طبيعة عملكم وأوقات الذروة، ما هي الأوقات أو المواسم الأنسب خلال العام للتمويل وشراء المعدات والملحقات للمهنة؟ وما السبب؟', 'season_table', 1, 8),
  (2, '9',  'هل توجد نقابة رسمية للمهنة، ومن هم الأشخاص الذين يعتبرون مرجعية أو شيوخ الكار في منطقتكم؟', 'multi_option', 1, 9),
  (2, '10', 'ماهي أفضل الماركات المتوفرة حاليا بالسوق للمعدات والآلات المستخدمة في المهنة؟', 'brand_table', 1, 10),
  (2, '11', 'ما هي الوسائل الأساسية التي تعتمدها حاليا لجذب الزبائن وزيادة عددهم؟', 'option_number', 1, 11),
  (3, '12', 'ماهي المعدات الأساسية للبدء بالمهنة، مع ذكر المعدات الحديثة في حال وجدت؟', 'equipment_table', 1, 12),
  (3, '13', 'ماهي التجهيزات الإضافية المطلوبة لافتتاح المشروع غير المعدات؟', 'option_number', 1, 13),
  (3, '14', 'ماهي المهارات الأساسية المطلوبة في المهنة ليستطيع الفرد مزاولة المهنة؟', 'text', 1, 14),
  (3, '15', 'ما هي أبرز التقنيات، المواد، الأساليب الجديدة، أو البرامج التي دخلت على هذه المهنة خلال السنوات الأخيرة؟', 'text', 1, 15),
  (4, '16', 'ماهي المشاكل والصعوبات التي تواجهك في عملك بالمهنة؟', 'challenge_table', 1, 16),
  (5, '17', 'ما هو الأسلوب الأفضل لتعليم وتدريب أساسيات المهنة؟', 'option_number', 1, 17),
  (5, '18', 'في حال توفر برامج أو دورات تدريبية حديثة متخصصة لتطوير مهنتك فنيا أو تقنيا، ماهي أبرز البرامج التي تقترحها؟', 'text', 1, 18),
  (5, '19', 'من لديه محتوى تدريبي او قادر على إعداد محتوى؟', 'participant_table', 1, 19),
  (5, '20', 'من لديه رغبة في أن يكون جزء من برنامج تدريب مهني منظم للمبتدئين؟', 'participant_table', 1, 20),
  (5, '21', 'من لديه الرغبة في الانضمام إلى دورة تدريبية تهدف إلى تطوير مهارات الإدارة المالية والقانونية والتسويقية لإدارة مشروعك الحالي؟', 'participant_table', 1, 21),
  (5, '22', 'من لديه الرغبة للمشاركة في بازار أو معرض من تنظيم المركز؟', 'participant_table', 1, 22)
ON DUPLICATE KEY UPDATE
  `Section_ID` = VALUES(`Section_ID`),
  `Text_Ar` = VALUES(`Text_Ar`),
  `Answer_Type` = VALUES(`Answer_Type`),
  `Is_Active` = VALUES(`Is_Active`),
  `Sort_Order` = VALUES(`Sort_Order`);

-- Seed fixed options. Question lookup is by Number to keep the insert stable.
INSERT INTO `profession_option` (`Question_ID`, `Text_Ar`, `Is_Other`, `Has_Details`, `Sort_Order`)
SELECT q.ID, o.Text_Ar, o.Is_Other, o.Has_Details, o.Sort_Order
FROM `profession_question` q
JOIN (
  SELECT '1' Number, 'دورات مهنية' Text_Ar, 0 Is_Other, 0 Has_Details, 1 Sort_Order
  UNION ALL SELECT '1', 'مهنة عائلية', 0, 0, 2
  UNION ALL SELECT '1', 'صانع', 0, 0, 3
  UNION ALL SELECT '2', 'أقل من 5 سنوات', 0, 0, 1
  UNION ALL SELECT '2', 'بين (5 - 10) سنوات', 0, 0, 2
  UNION ALL SELECT '2', 'أكثر من 10 سنوات', 0, 0, 3
  UNION ALL SELECT '3', 'بين (1 - 6) أشهر', 0, 0, 1
  UNION ALL SELECT '3', 'بين (6 - 12) أشهر', 0, 0, 2
  UNION ALL SELECT '3', 'أكثر من 12 شهر', 0, 0, 3
  UNION ALL SELECT '4', 'الأفراد والعائلات', 0, 0, 1
  UNION ALL SELECT '4', 'المحلات التجارية', 0, 0, 2
  UNION ALL SELECT '4', 'ورشات ومهنيون آخرون', 0, 0, 3
  UNION ALL SELECT '4', 'الشركات والمؤسسات', 0, 0, 4
  UNION ALL SELECT '4', 'تجار الجملة والموزعون', 0, 0, 5
  UNION ALL SELECT '4', 'أبناء الحي أو المنطقة', 0, 0, 6
  UNION ALL SELECT '4', 'زبائن من خارج المنطقة', 0, 0, 7
  UNION ALL SELECT '4', 'للذكور', 0, 0, 8
  UNION ALL SELECT '4', 'للإناث فقط', 0, 0, 9
  UNION ALL SELECT '4', 'غير ذلك', 1, 1, 10
  UNION ALL SELECT '7', 'جودة العمل والاتقان', 0, 0, 1
  UNION ALL SELECT '7', 'السعر المناسب والمنافس', 0, 0, 2
  UNION ALL SELECT '7', 'سرعة الإنجاز والتسليم', 0, 0, 3
  UNION ALL SELECT '7', 'موقع المحل او الورشة', 0, 0, 4
  UNION ALL SELECT '7', 'العلاقات الشخصية والسمعة', 0, 0, 5
  UNION ALL SELECT '7', 'غير ذلك', 1, 1, 6
  UNION ALL SELECT '9', 'توجد نقابة نشطة تمثل المهنة', 0, 1, 1
  UNION ALL SELECT '9', 'توجد مرجعيات أو شيوخ كار', 0, 1, 2
  UNION ALL SELECT '9', 'لا توجد نقابة ولا مرجعية واضحة', 0, 0, 3
  UNION ALL SELECT '11', 'السمعة الطيبة وتوصيات الزبائن الحاليين', 0, 0, 1
  UNION ALL SELECT '11', 'موقع المحل أو الورشة', 0, 0, 2
  UNION ALL SELECT '11', 'العلاقات الشخصية والمعارف', 0, 0, 3
  UNION ALL SELECT '11', 'وسائل التواصل الاجتماعي', 0, 0, 4
  UNION ALL SELECT '11', 'تقديم عروض خاصة أو حسومات أو أسعار منافسة', 0, 0, 5
  UNION ALL SELECT '11', 'التعاون والتشبيك مع مهنيين وحرفيين آخرين', 0, 0, 6
  UNION ALL SELECT '11', 'الإعلانات التقليدية', 0, 0, 7
  UNION ALL SELECT '11', 'غير ذلك', 1, 1, 8
  UNION ALL SELECT '13', 'مصادر طاقة بديلة', 0, 0, 1
  UNION ALL SELECT '13', 'مواد أولية', 0, 0, 2
  UNION ALL SELECT '13', 'ديكور', 0, 0, 3
  UNION ALL SELECT '13', 'آجار محل او الورشة', 0, 0, 4
  UNION ALL SELECT '17', 'تدريب جماعي داخل الورشة', 0, 0, 1
  UNION ALL SELECT '17', 'تدريب فردي داخل الورشة', 0, 0, 2
  UNION ALL SELECT '17', 'تدريب مهني أونلاين', 0, 0, 3
  UNION ALL SELECT '17', 'تدريب ضمن مركز تدريبي متخصص', 0, 0, 4
) o ON o.Number = q.Number
ON DUPLICATE KEY UPDATE
  `Is_Other` = VALUES(`Is_Other`),
  `Has_Details` = VALUES(`Has_Details`),
  `Sort_Order` = VALUES(`Sort_Order`);


