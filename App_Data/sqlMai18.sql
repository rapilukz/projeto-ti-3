select * from INFORMATION_SCHEMA.tables;
--select * from clientes;
--select * from carros;
--select * from alugueres order by idcli,idcar;

--delete from alugueres where idcli=18;
--delete from  alugueres where (idcli =16 and idcar >8) or (idcli =20 and idcar <8);

--select
select * from clientes where nome like 'P%';
select * from clientes where nome like '[PM]%';
select * from clientes where nome like '[^PM]%';
select * from clientes where nome like '%h%';
select * from clientes where len(nome)=6;
select * from clientes where nome like 'M_____';
select top  4 * from clientes order by idade desc;

select * from (select ROW_NUMBER() over (order by idade desc) Ordem, * from clientes)t
where t.Ordem between 3 and 4;

with cte as (
  select ROW_NUMBER() over (order by idade desc) Ordem, * from clientes
)
select * from cte where cte.Ordem between 3 and 4;

select ROW_NUMBER() over (partition by categoria order by idade desc) Ordem, * from clientes;

select * from clientes order by idade desc
offset 2 rows fetch next 2 rows only;

select * from clientes where idcli=16 or idcli=19 or idcli=21;
select * from clientes where idcli in(16,19,21);
select * from clientes where idcli in (select distinct idcli from alugueres);

select * from clientes where idcli in 
                 (select  idcli  from alugueres group by idcli  having count(*)=3);
--16,17,19,20,21
select * from clientes where idcli <> all(select distinct idcli from alugueres);
select * from clientes where idcli <> any(select distinct idcli from alugueres);

select * from clientes;

select * from clientes c where exists(
  select * from alugueres where idcli =c.idcli
);
select * from clientes c where  not exists(
  select * from alugueres where idcli =c.idcli
);


select * from clientes;
--project 
select * from clientes;
select idcli, nome from clientes;

select * from (
     select *, iif(idade>50,'Sénior','Júnior') as Classe from clientes)t
where Classe like 'Sénior';

select * , (
  case 
    when idcli % 3 =0 then 'Zero'
    when idcli % 3 =1 then 'Um'
    else 'Dois'
  end
) as Resto from clientes order by Resto ;

GO
 create function fn_apelido(@nome nvarchar(max))returns nvarchar(max)
 AS
 BEGIN
   declare @idx int = charindex(' ', reverse(@nome));
   if(@idx =0)return @nome;
   return right(@nome,@idx-1);
 END
GO
select dbo.fn_apelido('José Manuel Neves');
select dbo.fn_apelido('José');
select * , dbo.fn_apelido(nome) Apelido from clientes;

--rename
select * into Nova from clientes;
sp_help 'Nova';
sp_rename 'Nova','nova';
select * from nova;
sp_rename 'nova.nome','graca','COLUMN';

select ca.nome, cb.nome from clientes ca, clientes as cb;
select ca.nome, cb.nome from clientes ca, clientes as cb where ca.idcli <> cb.idcli;
select ca.nome, cb.nome from clientes ca, clientes as cb where ca.idcli > cb.idcli;

select * into alugas from alugueres;
select * from alugas;
insert into alugas (idcli, idcar, tempo, custo, dataaluguer)
values (100,10,2.5,60.00,'2024-05-18T14:13:00');
-- cross join
  select * from clientes c,alugas a;
--join
-- inner join
  select * from clientes c,alugas a where c.idcli <> a.idcli;
  select * from clientes c,alugas a where c.idcli = a.idcli;
  select * from clientes c inner join alugas a on c.idcli=a.idcli;
--  left join
   select * from clientes c left join alugas a on c.idcli=a.idcli;
 -- right join
   select * from clientes c right join alugas a on c.idcli=a.idcli;
-- full outer join
  select * from clientes c full outer join alugas a on c.idcli=a.idcli;
 --semi join
   select distinct c.* from clientes c inner join alugas a on c.idcli=a.idcli;
   select * from clientes c where exists(
       select * from alugas where idcli =c.idcli
   )
  select * from clientes where idcli in(select idcli from alugueres);
  
  --anti join
    select distinct c.* from clientes c left join alugas a on c.idcli=a.idcli where idal is null;
  
    select * from clientes c where not exists(
       select * from alugas where idcli =c.idcli
   )
   select * from clientes where idcli not in(select idcli from alugueres);
--union
  select * from clientes where idcli between 16 and 19 ;
  select * from clientes where idcli between 19 and 21 ;

   select * from clientes where idcli between 16 and 19 
   union
   select * from clientes where idcli between 19 and 21 ;

   select * from clientes where idcli between 16 and 19 
   union all
   select * from clientes where idcli between 19 and 21 ;
   -- sql recursivo
   update clientes set boss = null where idcli =18;


   with cte as (
      select idcli,nome,categoria,datanasc, idade, boss, 10 as Grau from clientes where boss  is NULL
      union all
      select c.idcli,c.nome,c.categoria,c.datanasc, c.idade, c.boss, (cte.Grau -1) as Grau 
                     from clientes c inner join cte on c.boss=cte.idcli
   )
   select * from cte;

  
  
  
  
  select * from clientes;


--intersect
 select * from clientes where idcli between 16 and 19 ;
   select * from clientes where idcli between 19 and 21 ;
  select * from clientes where idcli between 16 and 19 
  intersect
  select * from clientes where idcli between 19 and 21 ;


--except
   select * from clientes where idcli between 16 and 19 ;
   select * from clientes where idcli between 19 and 21 ;
  select * from clientes where idcli between 16 and 19 
  except
  select * from clientes where idcli between 19 and 21 ;

--division
select  * from clientes where not exists(
   select idcar from carros
   except
   select idcar from alugueres where idcli=clientes.idcli
);
----------------------------------------------------------------------
select * from clientes where idcli not  in (select idcli from alugueres)


select * from clientes where idcli in (select idcli from alugueres)
except
select  * from clientes where not exists(
   select idcar from carros
   except
   select idcar from alugueres where idcli=clientes.idcli
);

select  * from clientes where not exists(
   select idcar from carros
   except
   select idcar from alugueres where idcli=clientes.idcli
);

--aggregation
select idcli, count(*)[Total de Alugueres] , sum(custo)Total ,avg(custo) Media from alugueres
               group by idcli;

select idcli, count(*)[Total de Alugueres] , sum(custo)Total ,avg(custo) Media from alugueres
           where idcli <>19    group by idcli;

select idcli, count(*)[Total de Alugueres] , sum(custo)Total ,avg(custo) Media from alugueres
               group by idcli having count(*)=5 ;

select idcli,idcar, count(*)[Total de Alugueres] , sum(custo)Total ,avg(custo) Media from alugueres
               group by cube(idcli,idcar);

select idcar, count(*)[Total de Alugueres] , sum(custo)Total ,avg(custo) Media from alugueres
               group by cube(idcli,idcar) having idcli is null;


select idcli, count(*)[Total de Alugueres] , sum(custo)Total ,avg(custo) Media from alugueres
               group by cube(idcli,idcar) having idcar is null;

select  count(*)[Total de Alugueres] , sum(custo)Total ,avg(custo) Media from alugueres
               group by cube(idcli,idcar) having idcar is null and idcli is null;

select idcli,idcar, count(*)[Total de Alugueres] , sum(custo)Total ,avg(custo) Media from alugueres
               group by ROLLUP(idcli,idcar);
               
select idcli,idcar, count(*)[Total de Alugueres] , sum(custo)Total ,avg(custo) Media from alugueres
               group by ROLLUP(idcar,idcli);

select c.*, t.[Total de Alugueres] ,t.Total, t.Media from clientes c
cross apply(
select idcli, count(*)[Total de Alugueres] , sum(custo)Total ,avg(custo) Media from alugueres  group by idcli)t
where c.idcli = t.idcli;

GO
 create function fn_estatistica(@idcli int)returns table
 AS
  return  (select idcli  ,count(*)[Total de Alugueres] , sum(custo)Total ,avg(custo) Media from alugueres
          where idcli =@idcli  group by idcli);
GO

select * from dbo.fn_estatistica(17);

select * from clientes c
cross apply dbo.fn_estatistica(c.idcli);

--pivot table
--update delete insert
--stored procedures

GO
declare @soma decimal(10,2);
select @soma =isnull(@soma,0) +  custo from alugueres ;
print(@soma);
GO

-- sql dinâmico
declare @nomes nvarchar(max);
select  @nomes=isnull(@nomes+ ',','') + QUOTENAME(nome) from clientes;
declare @sql nvarchar(max) ='select * from( select nome, custo from clientes c inner join alugueres a on c.idcli=a.idcli)t 
               pivot(sum(custo) for nome in('+ @nomes +'))pvt;';

exec(@sql);
---------------------------------

GO
 alter proc sp_one(
  @limite int =10,
  @resposta nvarchar(max) out
 )
 AS
 BEGIN
  declare @i int=0, @sorte int;
  set @sorte = floor(rand() *100)+1;
  print('Sorte:' +  cast(@sorte as nvarchar(20)));
  if @sorte %2 =0 
          BEGIN
             print('Par');
          END
  else
          BEGIN
             print('ímpar')
          END

  set @resposta =(
     case 
      when @sorte between 0 and 20 then 'Mau'
      when @sorte between 21 and 49 then 'Insuficiente'
      else 'Suficiente'
     end   
  );

  declare @soma int =0;
  while @i < @limite 
  begin
     set @soma +=@i;
     set @i+=1;
     print @i;
  end
  return @soma;
 END
GO
declare @r  nvarchar(max), @s int;
exec @s =sp_one @limite=10, @resposta = @r out;
select @r as Resposta, @s as Soma;
--functions 

select * from dbo.xpto();

GO
 create proc sp_alugueres(
   @idcli int =NULL,
   @media decimal(10,2) out,
   @soma decimal(10,2) out
 )
 AS
 BEGIN
   select * from alugueres where idcli = @idcli or @idcli is NULL;
   declare @total int =@@rowcount;
   select @media=avg(custo), @soma=sum(custo) from alugueres where idcli = @idcli or @idcli is NULL;
   return @total;
 END
GO
declare @t int, @m decimal(10,2), @s decimal(10,2);
exec @t= sp_alugueres @idcli= NULL, @media = @m out ,@soma=@s out;
select @t [Total de Registos], @m Media , @s Soma;

select * into nova from clientes;
select * from nova;
--triggers
drop trigger trg_one;
GO
  create trigger trg_one
  on nova
  instead of insert ,delete,update
  AS
  BEGIN
    select * from inserted;
    select * from deleted;
  END
GO
delete from nova where idcli =16;
update nova set graca ='Zé Carioca' where idcli=16;
insert into nova ( graca,categoria, datanasc, idade, boss)
values ('Donalda','Xpto','1998-10-11',23,NULL);
select * from nova;


--
select * from nova;

GO
create trigger trg_del
on nova
for delete
AS
BEGIN
   if not exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME like 'historico')
   BEGIN
      CREATE TABLE [dbo].[historico] (
            [idcli]     INT             NOT NULL,
            [graca]     NVARCHAR (50)   NOT NULL,
            [categoria] NVARCHAR (50)   NULL,
            [datanasc]  DATE            NULL,
            [idade]     NUMERIC (18, 6) NULL,
            [boss]      INT             NULL
      );
   END
   insert into historico(idcli,graca,categoria, datanasc,idade,boss)
   select idcli,graca,categoria, datanasc,idade,boss from deleted;
END
  
GO


select * from nova;
select * from historico;
delete from nova where idcli <=18;

select * from alugueres;
insert into alugueres (idcli,idcar,tempo, dataaluguer)
values (16,9,1,getdate())
update alugueres set tempo =1000 where idal=32;

GO
 create trigger trg_custo
 on alugueres
 for update, insert
 AS
 BEGIN
   update   alugueres set custo= ca.phora * i.tempo from alugueres a inner join carros ca on ca.idcar= a.idcar
                                        inner join inserted i on a.idal=i.idal;
 END
GO







