CREATE TABLE Pro_BillCode(
    Id                             TEXT             NOT NULL,
    BusiCode                       TEXT             NOT NULL,
    BillCode                       TEXT,
    Remarks                        TEXT,
    CONSTRAINT Pro_BillCode_PK PRIMARY KEY (Id)
)
;





CREATE TABLE Pro_Follow(
    Id                             TEXT             NOT NULL,
    FollowDate                     TEXT             NOT NULL,
    Address                        TEXT             NOT NULL,
    Designer                       TEXT             NOT NULL,
    Adviser                        TEXT             NOT NULL,
    Follower                       TEXT             NOT NULL,
    CONSTRAINT Pro_Follow_PK PRIMARY KEY (Id)
)
;





CREATE TABLE Pro_FollowDet(
    Id                             TEXT             NOT NULL,
    ParentId                       TEXT             NOT NULL,
    FollowDate                     TEXT             NOT NULL,
    Remarks                        TEXT             NOT NULL,
    CONSTRAINT Pro_FollowDet_PK PRIMARY KEY (Id)
)
;





CREATE TABLE Pro_Construct(
    Id                             TEXT             NOT NULL,
    BillCode                       TEXT             NOT NULL,
    CusName                        TEXT             NOT NULL,
    CusPhone                       TEXT             NOT NULL,
    ProMinister                    TEXT             NOT NULL,
    ProMinisterPhone               TEXT             NOT NULL,
    Address                        TEXT             NOT NULL,
    ConstructDate                  TEXT             NOT NULL,
    AlreadyConstruction            TEXT             NOT NULL,
    Matters                        TEXT,
    Remarks                        TEXT,
    CONSTRAINT Pro_Construct_PK PRIMARY KEY (Id)
)
;





CREATE TABLE Pro_ConstructDet(
    Id                             TEXT             NOT NULL,
    ParentId                       TEXT             NOT NULL,
    Name                           TEXT             NOT NULL,
    Region                         TEXT             NOT NULL,
    Area                           TEXT             NOT NULL,
    Price                          TEXT             NOT NULL,
    Model                          TEXT             NOT NULL,
    Spec                           TEXT             NOT NULL,
    CONSTRAINT Pro_ConstructDet_PK PRIMARY KEY (Id)
)
;





CREATE TABLE Pro_ConstructAtt(
    Id                             TEXT             NOT NULL,
    ParentId                       TEXT             NOT NULL,
    Url                            TEXT             NOT NULL,
    Name                           TEXT             NOT NULL,
    CONSTRAINT Pro_ConstructAtt_PK PRIMARY KEY (Id)
)
;





CREATE TABLE Pro_ConstructionSite(
    Id                             TEXT             NOT NULL,
    Address                        TEXT             NOT NULL,
    AddressRemarks                 TEXT,
    CusName                        TEXT             NOT NULL,
    CusNameRemarks                 TEXT,
    Designer                       TEXT             NOT NULL,
    DesignerRemarks                TEXT,
    ProMinister                    TEXT             NOT NULL,
    ProMinisterRemarks             TEXT,
    Checker                        TEXT             NOT NULL,
    CheckerRemarks                 TEXT,
    StartDate                      TEXT             NOT NULL,
    EndDate                        TEXT             NOT NULL,
    Remarks                        TEXT,
    CONSTRAINT Pro_ConstructionSite_PK PRIMARY KEY (Id)
)
;






