ALTER TABLE Pipeline
    DROP CONSTRAINT UK_Pipeline_ProjectId_SourceUrl_SourceRevision

EXEC sp_rename
    @objname = 'Pipeline.SourceUrl',
    @newname = 'BranchName',
    @objtype = 'COLUMN'    

EXEC sp_rename
    @objname = 'Pipeline.SourceRevision',
    @newname = 'Revision',
    @objtype = 'COLUMN'

ALTER TABLE Pipeline
    ADD CONSTRAINT UK_Pipeline_ProjectId_SourceUrl_SourceRevision UNIQUE (ProjectId, BranchName, Revision)