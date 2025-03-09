insert into atmapp.account (Login, Pin, Holder, Balance, Status) values ('admin123', 12345, 'admin', 0, 1);
insert into atmapp.account (Login, Pin, Holder, Balance, Status) values ('user123', 54321, 'example', 5000, 1);
delete from atmapp.account where accountnum= 1;
select * from atmapp.account;