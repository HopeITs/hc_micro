-- Apply separately to each governorate database before using person review.
CREATE TABLE IF NOT EXISTS kobo_person_review (
 Submission_ID INT NOT NULL PRIMARY KEY,
 Project_ID INT NOT NULL,
 Source_Hash CHAR(64) NOT NULL,
 Kobo_Name TEXT NOT NULL,
 Person_ID INT NULL,
 Relationship_Hash CHAR(64) NULL,
 State VARCHAR(20) NOT NULL DEFAULT 'pending',
 Revision INT NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
CREATE TABLE IF NOT EXISTS kobo_person_review_audit (
 ID INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Submission_ID INT NOT NULL,
 Project_ID INT NOT NULL,
 Person_ID INT NULL,
 Decision VARCHAR(20) NOT NULL,
 Reason TEXT NOT NULL,
 Actor VARCHAR(255) NOT NULL,
 Actor_ID INT NOT NULL,
 Recorded_UTC DATETIME NOT NULL,
 Source_Hash CHAR(64) NOT NULL,
 Relationship_Hash CHAR(64) NULL,
 Kobo_Name TEXT NOT NULL,
 Candidate_JSON LONGTEXT NOT NULL,
 KEY (Submission_ID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
