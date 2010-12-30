CREATE TABLE Pipeline
(
    PipelineId    int    NOT NULL
        IDENTITY(1, 1)
        CONSTRAINT PK_Pipeline PRIMARY KEY CLUSTERED
    , ProjectId    int    NOT NULL
        CONSTRAINT FK_Pipeline_Project FOREIGN KEY REFERENCES Project (ProjectId)
    , SourceUrl    nvarchar(256)    NOT NULL
    , SourceRevision    int    NOT NULL
    , Created    datetime    NOT NULL
        CONSTRAINT DF_Pipeline_Created DEFAULT GETDATE()
    , Modified    datetime    NOT NULL
        CONSTRAINT DF_Pipeline_Modified DEFAULT GETDATE()
    , CONSTRAINT UK_Pipeline_ProjectId_SourceUrl_SourceRevision UNIQUE (ProjectId, SourceUrl, SourceRevision)
)

GO

SET IDENTITY_INSERT Pipeline ON

INSERT INTO Pipeline
(PipelineId, ProjectId, SourceUrl, SourceRevision, Created, Modified)
SELECT * FROM Build

SET IDENTITY_INSERT Pipeline OFF

GO

ALTER TABLE BuildStep
    ADD
        PipelineId    int    NULL
            CONSTRAINT FK_BuildStep_Pipeline FOREIGN KEY REFERENCES Pipeline (PipelineId)

GO

UPDATE BuildStep
SET PipelineId = BuildId

GO

ALTER TABLE BuildStep
    ALTER
        COLUMN PipelineId    int    NOT NULL

GO

ALTER TABLE BuildStep
    ALTER
        COLUMN BuildId    int    NULL