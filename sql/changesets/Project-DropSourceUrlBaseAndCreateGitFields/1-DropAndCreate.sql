ALTER TABLE Project
    DROP COLUMN SourceUrlBase

ALTER TABLE Project
    ADD  GitRemoteUrl  nvarchar(200)  NULL

ALTER TABLE Project
    ADD  GithubUrl  nvarchar(200)  NULL
