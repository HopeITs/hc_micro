-- Selected database only. This prepares stage mappings; it does not enable importing.
-- Final is M4, never legacy M5 or Closing. Blank unresolved rows deliberately fail validation.
CREATE TABLE IF NOT EXISTS kobo_monitoring_stage_mapping (
  Kobo_Form_UID varchar(100) COLLATE utf8mb4_bin NOT NULL PRIMARY KEY,
  Stage_Field varchar(255) NOT NULL DEFAULT '',
  Fixed_Stage tinyint NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
CREATE TEMPORARY TABLE tmp_kobo_monitoring_stages (
  LocalDatabase varchar(100), Kobo_Form_UID varchar(100), Stage_Field varchar(255), Fixed_Stage tinyint NULL
);
INSERT INTO tmp_kobo_monitoring_stages VALUES
('hcsyria_micro', 'ay2c9afff5StTkQdneHHUS', '__010', NULL),
('hcsyria_micro', 'a3BwzyULUpD3P8jh9B8ADn', 'vist_no', NULL),
('hcsyria_micro', 'ai6SnMT3dqZAEELCfesSqp', 'vist_', NULL),
('hcsyria_micro', 'aDGPm94uwS7HYk4LcG46oh', '', 4),
('hcsyria_micro', 'aL3TGhp5wAhuPseTMhEksd', '', NULL),
('hcsyria_micro', 'aRwMM45vMjfL7Y5jSYnVsD', '', NULL),
('hcsyria_micro_d', 'aFqwLzjDcQ4gczMdfUaXNq', '__010', NULL),
('hcsyria_micro_d', 'atB8PhFWVmYQskk5rNnehB', 'vist_no', NULL),
('hcsyria_micro_d', 'aZzfVwdN9sFyBCWDzBVqaU', 'vist_', NULL),
('hcsyria_micro_d', 'aYCc7VCGeP77BE9zUTMceV', '', 4),
('hcsyria_micro_h', 'ayz6LYxMSyePXdfs27cJja', '__010', NULL),
('hcsyria_micro_h', 'acHufLsXu5jQQscMhtyeLt', 'vist_no', NULL),
('hcsyria_micro_h', 'aj4fsVSAoDQDtpt4act3mP', 'vist_', NULL),
('hcsyria_micro_h', 'a3DSoAPvHiuEuXzPbVPaJD', '', 4),
('hcsyria_micro_h', 'aBk3WrFmRbWuZrPgU3Jcya', '', NULL),
('hcsyria_micro_h', 'akPJmrm2KVbs4LoKzYF623', '', NULL),
('hcsyria_micro_l', 'a9yjLmEjdpvxcSuUzbEu3m', 'vist_no', NULL),
('hcsyria_micro_l', 'aMdK7mGpanWHyF39Reu498', 'vist_', NULL),
('hcsyria_micro_l', 'a4bFkLTC2iRwht3vkTdbuo', '', 4),
('hcsyria_micro_w', 'aRwLqiUq5LKhjF4bWH4tcp', '__010', NULL),
('hcsyria_micro_w', 'aZjS5bHmMPARXPRp5Hbr5d', 'vist_no', NULL),
('hcsyria_micro_w', 'arvdKcen8mFrYkYcWKYRmJ', 'vist_', NULL),
('hcsyria_micro_w', 'aLiFSfoPAHBjFQRD7eswrL', '', 4),
('hcsyria_micro_w', 'aqN95K2Zoja2iywSYXfqQT', '', NULL),
('hcsyria_micro_w', 'ajYnR9RaUjDb9GSFPbWa25', '', NULL);
INSERT IGNORE INTO kobo_monitoring_stage_mapping (Kobo_Form_UID, Stage_Field, Fixed_Stage)
SELECT s.Kobo_Form_UID, s.Stage_Field, s.Fixed_Stage FROM tmp_kobo_monitoring_stages s
JOIN kobo_form_mapping f ON BINARY f.Kobo_Form_UID = BINARY s.Kobo_Form_UID
WHERE s.LocalDatabase = DATABASE() AND f.LocalDatabase = DATABASE();
DROP TEMPORARY TABLE tmp_kobo_monitoring_stages;
-- Reruns preserve manually reviewed configurations.
