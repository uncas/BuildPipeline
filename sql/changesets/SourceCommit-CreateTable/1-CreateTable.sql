CREATE TABLE SourceCommit
(
    SourceCommitId  int  IDENTITY(1,1)  NOT NULL
        CONSTRAINT PK_SourceCommit PRIMARY KEY CLUSTERED
    , ProjectId  int  NOT NULL
    , Revision  varchar(100)  NOT NULL
    , Subject  nvarchar(100)  NOT NULL
    , AuthorName  nvarchar(50)  NOT NULL
    , AuthorEmail  nvarchar(100)  NULL
    , AuthorDate  datetime  NOT NULL
    , CONSTRAINT FK_SourceCommit_Project FOREIGN KEY (ProjectId) REFERENCES Project(ProjectId)
    , CONSTRAINT UK_SourceCommit_ProjectRevision UNIQUE(ProjectId, Revision)
)