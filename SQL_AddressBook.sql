--Welcome to Advance Addressbook Program
create database Addressbook
create table Family(
FirstName varchar(50) not null primary key,
LastName varchar(50),
Address varchar(100),
City varchar(25),
State varchar(25),
ZipCode varchar(6),
PhoneNumber varchar(10),
Email varchar(50)
)

select * from Family
truncate table Family
delete from Family where FirstName = 'Tushar'
exec dbo.ContactExists 'Tushar'