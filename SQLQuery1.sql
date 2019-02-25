use RCA_Archive 
go
alter role db_owner drop member Tactical_ome;
go
alter role db_datawriter add member Tactical_ome;
go
alter role db_datareader add member Tactical_ome;
go
grant execute to Tactical_ome;
go
grant select, insert, update, delete on schema :: dbo to Tactical_ome;
go


use RCA_Live 
go
alter role db_owner drop member Tactical_ome;
go
alter role db_datawriter add member Tactical_ome;
go
alter role db_datareader add member Tactical_ome;
go
grant execute to Tactical_ome;
go
grant select, insert, update, delete on schema :: dbo to Tactical_ome;
go


use RCA_Snapshot 
go
alter role db_owner drop member Tactical_ome;
go
alter role db_datawriter add member Tactical_ome;
go
alter role db_datareader add member Tactical_ome;
go
grant execute to Tactical_ome;
go
grant select, insert, update, delete on schema :: dbo to Tactical_ome;
go


use Tactical_OME
go
alter role db_owner drop member Tactical_ome;
go
alter role db_datawriter add member Tactical_ome;
go
alter role db_datareader add member Tactical_ome;
go
grant execute to Tactical_ome;
go
grant select, insert, update, delete on schema :: dbo to Tactical_ome;
go


use TOME_Audit
go
alter role db_owner drop member Tactical_ome;
go
alter role db_datawriter add member Tactical_ome;
go
alter role db_datareader add member Tactical_ome;
go
grant execute to Tactical_ome;
go
grant select, insert, update, delete on schema :: dbo to Tactical_ome;
go