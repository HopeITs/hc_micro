-- Selected governorate database. Apply the monitoring upgrades first.
CREATE TABLE IF NOT EXISTS kobo_sync_conflict (
 ID int NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Incoming_ID int NOT NULL,
 Existing_ID int NOT NULL,
 Visit_ID int NOT NULL,
 Incoming_Hash char(64) NOT NULL,
 Existing_Hash char(64) NOT NULL,
 State varchar(20) NOT NULL DEFAULT 'open',
 Revision int NOT NULL DEFAULT 0,
 Decision varchar(20) NULL,
 Reason text NULL,
 Decided_By varchar(200) NULL,
 Decided_User_ID int NULL,
 Decided_At datetime NULL,
 Applied_At datetime NULL,
 UNIQUE KEY uq_kobo_conflict_incoming (Incoming_ID),
 KEY idx_kobo_conflict_existing (Existing_ID),
 CONSTRAINT fk_kobo_conflict_incoming FOREIGN KEY (Incoming_ID) REFERENCES kobo_submission(ID),
 CONSTRAINT fk_kobo_conflict_existing FOREIGN KEY (Existing_ID) REFERENCES kobo_submission(ID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
CREATE TABLE IF NOT EXISTS kobo_sync_conflict_audit (
 ID int NOT NULL AUTO_INCREMENT PRIMARY KEY,
 Conflict_ID int NOT NULL,
 Event varchar(30) NOT NULL,
 Decision varchar(20) NOT NULL,
 Reason text NOT NULL,
 Actor varchar(200) NOT NULL,
 Actor_User_ID int NOT NULL,
 Recorded_At datetime NOT NULL,
 Incoming_JSON longtext NOT NULL,
 Existing_JSON longtext NOT NULL,
 KEY idx_kobo_conflict_audit (Conflict_ID),
 CONSTRAINT fk_kobo_conflict_audit FOREIGN KEY (Conflict_ID) REFERENCES kobo_sync_conflict(ID)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
