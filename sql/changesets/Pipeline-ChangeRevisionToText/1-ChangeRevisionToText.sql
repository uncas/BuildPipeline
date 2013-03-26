ALTER TABLE Pipeline
    DROP CONSTRAINT UK_Pipeline_ProjectId_SourceUrl_SourceRevision

    
ALTER TABLE Pipeline
    ALTER COLUMN  SourceRevision  varchar(100)  NOT NULL


ALTER TABLE Pipeline
    ADD CONSTRAINT [UK_Pipeline_ProjectId_SourceUrl_SourceRevision] UNIQUE NONCLUSTERED 
(
	[ProjectId] ASC,
	[SourceUrl] ASC,
	[SourceRevision] ASC
)
