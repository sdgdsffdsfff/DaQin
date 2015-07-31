CREATE TABLE Sys_User(
    Id                             TEXT             NOT NULL,
    UserName                       TEXT             NOT NULL,
    Pwd                            TEXT             NOT NULL,
    ShowName                       TEXT             NOT NULL,
    Remarks                        TEXT,
    CONSTRAINT Sys_User_PK PRIMARY KEY (Id)
)
;





CREATE TABLE Sys_Role(
    Id                             TEXT             NOT NULL,
    RoleName                       TEXT             NOT NULL,
    Remarks                        TEXT,
    CONSTRAINT Sys_Role_PK PRIMARY KEY (Id)
)
;





CREATE TABLE Sys_UserRole(
    Id                             TEXT             NOT NULL,
    UserId                         TEXT             NOT NULL,
    RoleId                         TEXT             NOT NULL,
    CONSTRAINT Sys_UserRole_PK PRIMARY KEY (Id)
)
;





CREATE TABLE Sys_Menu(
    Id                             TEXT             NOT NULL,
    Name                           TEXT             NOT NULL,
    FunctionId                     TEXT             NOT NULL,
    Url                            TEXT,
    Sort                           TEXT             NOT NULL,
    Pid                            TEXT             NOT NULL,
    CONSTRAINT Sys_Menu_PK PRIMARY KEY (Id)
)
;





CREATE TABLE Sys_Function(
    Id                             TEXT             NOT NULL,
    Name                           TEXT             NOT NULL,
    Controller                     TEXT             NOT NULL,
    Action                         TEXT,
    Pid                            TEXT             NOT NULL,
    Sort                           TEXT             NOT NULL,
    CONSTRAINT Sys_Function_PK PRIMARY KEY (Id)
)
;





CREATE TABLE Sys_RoleFunction(
    Id                             TEXT             NOT NULL,
    RoleId                         TEXT             NOT NULL,
    FunctionId                     TEXT             NOT NULL,
    CONSTRAINT Sys_RoleFunction_PK PRIMARY KEY (Id)
)
;






