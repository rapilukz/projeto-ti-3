--select * from INFORMATION_SCHEMA.TABLES;
--drop table [Table];

--create table clientes(
-- idcli int identity(10,1)primary key,
-- nome nvarchar(50) not null,
-- categoria nvarchar(50) 
--   check(categoria = 'Alfa' or categoria ='Bravo' or categoria='Charlie')
--   default 'Charlie',
-- datanasc date,
-- idade as  datediff(day,datanasc,getdate())/365.25   
--);

--insert into clientes(nome,categoria,datanasc)
--values
--('Tio Patinhas','Alfa','1963-05-10'),
--('Margarida','Charlie','2003-08-14'),
--('Pato Donald','Bravo','1999-04-10'),
--('Pateta','Bravo','1999-05-12'),
--('Mickey','Alfa','1999-05-11'),
--('Minnie','Bravo','2001-11-10');

-- delete from clientes where nome like 'Pato%';
--set identity_insert clientes ON;
--insert into clientes (idcli,nome,datanasc)
--values(18,'Pato Donald','1991-01-21');
--set identity_insert clientes OFF;

--alter table clientes add boss int constraint
--         fkclientesclientes foreign key  references clientes(idcli);

select * from clientes;

--update clientes set boss =(
--  case idcli
--	  when 18 then 16
--	  when 20 then 16
--	  when 21 then 20
--	  else 18
--  end
--);

--create table carros (
--  idcar int identity primary key,
--  modelo nvarchar(50)not null,
--  phora decimal(10,2) check(phora between 0 and 100)
--);

--insert into carros 
--values
--('Ford Fiesta',23.50),
--('Ford Focus',13.50),
--('Fiat Punto',33.50),
--('Fiat 500',33.00),
--('Ferrari F40',100.00);

--select * from carros;

--create table marcas(
--  marca nvarchar(50)primary key
--);

--insert into marcas
--values ('Ford'),('Fiat'),('Ferrari'),('Porsche'),('Mercedes');

select * from marcas;

go
create function fn_marca(@modelo as nvarchar(max))returns nvarchar(max)
AS
begin
   declare @i int = charindex(' ', @modelo);
   return left(@modelo, @i);
end
go

alter table carros add marca nvarchar(50) constraint fkmarcascarros foreign key
                                       references marcas(marca);

sp_help 'carros';

--update carros set marca = dbo.fn_marca(modelo);

--select * from carros;

--create table alugueres (
--  idal int identity primary key,
--  idcli int constraint fkclientesalugueres foreign key references clientes(idcli)
--                     on update cascade on delete cascade,
--  idcar int constraint fkcarrosalugueres foreign key references carros(idcar),
--  tempo decimal(10,2),
--  custo decimal(10,2),
--  dataaluguer datetime default getdate(),
--  constraint ucaluguer unique(idcli,idcar,dataaluguer)
--);

--insert into alugueres(idcli,idcar)
--select c.idcli, idcar from clientes c, carros ca ;

select * from alugueres;

update alugueres set tempo =rand()* 100;

go
  create view v_tombola 
  as
  select rand() as sorte;
go

select sorte from v_tombola;

go
 create function fn_tempo(@min int, @max int)returns decimal(10,2)
 AS
 begin
  declare @delta int = @max-@min +1;
  return ((select sorte from v_tombola) * @delta) +@min
 end
go

update alugueres set tempo = dbo.fn_tempo(1,100);
select * from alugueres;

select a.tempo, c.phora , a.custo from alugueres a inner join carros c on c.idcar=a.idcar;


update alugueres set custo = a.tempo * c.phora 
from alugueres a inner join carros c on c.idcar=a.idcar;


--select
--project
--rename
--join inner join left join right join full outer join semi-join anti join
--union
--union all
--except
--intersect
--division
--aggregation rollup cube
--pivot table
--sql recursive
--rownumber
--skip
--limit
--cross apply
--exists
--stored procedure
--triggers
--functions
--views























