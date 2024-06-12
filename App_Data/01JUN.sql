delete from alugueres where idcar =9;
select * from carros;
alter table carros add fotopath nvarchar(255);

GO
create view v_carros
AS
select c.* ,t.total from carros c
left join
(select idcar, count(*) total from alugueres group by idcar)t
on   c.idcar= t.idcar;
GO