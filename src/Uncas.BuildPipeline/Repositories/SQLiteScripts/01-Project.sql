CREATE TABLE Project
(
    ProjectId    integer    NOT NULL
        PRIMARY KEY ASC
    , ProjectName    text    NOT NULL
        UNIQUE
    , SourceUrlBase    text    NOT NULL
)
