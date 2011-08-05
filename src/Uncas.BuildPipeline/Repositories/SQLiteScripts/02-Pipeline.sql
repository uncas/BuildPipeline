CREATE TABLE Pipeline
(
    PipelineId    integer    NOT NULL
        PRIMARY KEY ASC
    , ProjectId    integer    NOT NULL
    , SourceUrl    text    NOT NULL
    , SourceRevision    integer    NOT NULL
    , Created    datetime    NOT NULL
    , Modified    datetime    NOT NULL
    , SourceAuthor    text    NOT NULL
    , PackagePath    text    NULL
    , FOREIGN KEY (ProjectId) REFERENCES Project (ProjectId)
)
