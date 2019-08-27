CREATE TABLE alibuyoffermst(
phid number(19) PRIMARY KEY,
appkey VARCHAR2(100) NOT NULL,
appsecret VARCHAR2(100) NOT NULL,
apptoken VARCHAR2(100) NOT NULL,
  offerid VARCHAR2(100) NULL,
          subject VARCHAR2(1000) NULL,
          phone VARCHAR2(100) NULL,
          contact VARCHAR2(1000) NULL,
         description  VARCHAR2(1000) NULL,
          gmtQuotationExpire DATE NULL,
          subBizType VARCHAR2(100) NULL,
          processTemplateCode VARCHAR2(100) NULL,
          transToolType VARCHAR2(100) NULL,
          invoiceRequirement VARCHAR2(100) NULL,
          visibleAfterEndQuote int NULL,
          sourceMethodType VARCHAR2(100) NULL,
         supplierMemberIds VARCHAR2(1000) NULL,
          includeTax int NULL,
          quoteHasPostFee int NULL,
         allowPartOffer int NULL,
        certificateIds VARCHAR2(1000) NULL,
       otherCertificateNames VARCHAR2(2000) NULL,
         subuserid VARCHAR2(100) NULL,
         ngid VARCHAR2(100) NULL,
         closereason VARCHAR2(1000) NULL,
        closedesc  VARCHAR2(1000) NULL,
        receiveStreetAddress VARCHAR2(1000) NULL

);
Create Sequence alibuyoffermstidentity
  Increment By 1
  Start With 1
  Nomaxvalue
  Nocycle
  Cache 10;
  
 Create Trigger tr_alibuyoffermst Before Insert On alibuyoffermst For Each Row
  Begin
  Select alibuyoffermstidentity.Nextval Into :New.phid From dual;
  End;

CREATE TABLE alibuyofferdtl(
phid number(19)  PRIMARY KEY,
appkey VARCHAR2(100) NOT NULL,
appsecret VARCHAR2(100) NOT NULL,
apptoken VARCHAR2(100) NOT NULL,
  offerid VARCHAR2(100) NULL,
           pritemid VARCHAR2(100) NULL,
       productCode VARCHAR2(100) NULL,
         purchaseAmount NUMERIC(18,6) NULL,
         subject VARCHAR2(1000) NULL,
        descript VARCHAR2(2000) NULL,
         unit VARCHAR2(100) NULL,
         brandName VARCHAR2(1000) NULL,
         modelNumber VARCHAR2(1000) NULL

);

ALTER TABLE alibuyofferdtl ADD purchaseNoteItemId VARCHAR2(100) Null;
ALTER TABLE alibuyoffermst ADD status VARCHAR2(100) Null;
ALTER TABLE alibuyoffermst ADD gmtcreate DATE Null;

Create Sequence alibuyofferdtlidentity
  Increment By 1
  Start With 1
  Nomaxvalue
  Nocycle
  Cache 10;
  
 Create Trigger tr_alibuyofferdtl Before Insert On alibuyofferdtl For Each Row
  Begin
  Select alibuyofferdtlidentity.Nextval Into :New.phid From dual;
  End;

CREATE TABLE quotation(
phid number(19)  PRIMARY KEY,
appkey VARCHAR2(100) NOT NULL,
appsecret VARCHAR2(100) NOT NULL,
apptoken VARCHAR2(100) NOT NULL,
quoteid VARCHAR2(100) NULL,
ngid VARCHAR2(100) NULL,
status VARCHAR2(100) NULL,
gmtcreate DATE NULL,
expiredate DATE NULL,
buyofferid VARCHAR2(100) NULL,
suppliermemberid VARCHAR2(100) NULL,
contact VARCHAR2(100) NULL,
mobile VARCHAR2(100) NULL,
invoicetype VARCHAR2(100) NULL,
freight number(19) NULL,
totalprice number(19) NULL,
paytype VARCHAR2(100) NULL
);
Create Sequence quotationidentity
  Increment By 1
  Start With 1
  Nomaxvalue
  Nocycle
  Cache 10;
  
 Create Trigger tr_quotation Before Insert On quotation For Each Row
  Begin
  Select quotationidentity.Nextval Into :New.phid From dual;
  End;
  

CREATE TABLE quotationdtl(
phid number(19)  PRIMARY KEY,
appkey VARCHAR2(100) NOT NULL,
appsecret VARCHAR2(100) NOT NULL,
apptoken VARCHAR2(100) NOT NULL,
quoteid VARCHAR2(100) NULL,
quotedid VARCHAR2(100) NULL,
pritemid VARCHAR2(100) NULL,
       productcode VARCHAR2(100) NULL,
         amount NUMERIC(18,6) NULL,
         price number(19) NULL,
         subject VARCHAR2(1000) NULL,
        itemcount NUMERIC(18,6) NULL,
         unit VARCHAR2(100) NULL,
         brandname VARCHAR2(1000) NULL,
         modelnumber VARCHAR2(1000) NULL,
         taxrate VARCHAR2(100) NULL
         
);
Create Sequence quotationdtlidentity
  Increment By 1
  Start With 1
  Nomaxvalue
  Nocycle
  Cache 10;
  
 Create Trigger tr_quotationdtl Before Insert On quotationdtl For Each Row
  Begin
  Select quotationdtlidentity.Nextval Into :New.phid From dual;
  End;
  
CREATE TABLE aliordermst(
phid number(19) PRIMARY KEY,
appkey VARCHAR2(100) NOT NULL,
appsecret VARCHAR2(100) NOT NULL,
apptoken VARCHAR2(100) NOT NULL,
orderid VARCHAR2(100) NULL,
status  VARCHAR2(100) NULL,
subbiz VARCHAR2(100) NULL,
gmtcreate DATE NULL,
discount number(19) NULL,
carriage number(19) NULL,
refundpayment number(19) NULL,
sumpayment number(19) NULL,
sumproductpayment number(19) NULL,
sellercompanyname VARCHAR2(1000) NULL,
selleremail VARCHAR2(1000) NULL,
sellermemberid VARCHAR2(100) NULL,
selleruserid number(19) NULL,
selleralipayid VARCHAR2(100) NULL,
sellerloginid VARCHAR2(100) NULL,
buyercompanyname VARCHAR2(1000) NULL,
buyeremail VARCHAR2(1000) NULL,
buyermemberid VARCHAR2(100) NULL,
buyeruserid number(19) NULL,
buyeralipayid VARCHAR2(100) NULL,
buyerloginid VARCHAR2(100) Null
);
Create Sequence aliordermstidentity
  Increment By 1
  Start With 1
  Nomaxvalue
  Nocycle
  Cache 10;
  
 Create Trigger tr_aliordermst Before Insert On aliordermst For Each Row
  Begin
  Select aliordermstidentity.Nextval Into :New.phid From dual;
  End;
  
CREATE TABLE aliorderdtl(
phid number(19) PRIMARY KEY,
appkey VARCHAR2(100) NOT NULL,
appsecret VARCHAR2(100) NOT NULL,
apptoken VARCHAR2(100) NOT NULL,
orderid VARCHAR2(100) NULL,
orderdtlid VARCHAR2(100) NULL,
productName VARCHAR2(1000) NULL,
unit VARCHAR2(100) NULL,
qty NUMERIC(18,6) NULL,
price number(19)  NULL,
amt number(19)  NULL,
refundFee number(19) NULL,
discount NUMERIC(18,6) NULL,
sourceid VARCHAR2(100) NULL,
status VARCHAR2(100) NULL,
actualpayfee number(19) Null
);

ALTER TABLE aliordermst ADD sellername VARCHAR2(100) Null;
ALTER TABLE aliordermst ADD buyername VARCHAR2(100) Null;

Create Sequence aliorderdtlidentity
  Increment By 1
  Start With 1
  Nomaxvalue
  Nocycle
  Cache 10;
  
 Create Trigger tr_aliorderdtl Before Insert On aliorderdtl For Each Row
  Begin
  Select aliorderdtlidentity.Nextval Into :New.phid From dual;
  End;
  

CREATE TABLE aliopbulksettlement(
phid number(19)  PRIMARY KEY,
appkey VARCHAR2(100) NOT NULL,
appsecret VARCHAR2(100) NOT NULL,
apptoken VARCHAR2(100) NOT NULL,
receiveid VARCHAR2(100) NULL,
status VARCHAR2(100) NULL,
actualpayfee number(19) NULL,
issuccess INT NULL,
orderid VARCHAR2(100) NULL,
gmtcreate DATE NULL,
gmtreceive DATE NULL,
selleruserid number(19) NULL,
sellercompanyname VARCHAR2(1000) NULL,
operatorusername VARCHAR2(1000) NULL,
buyeruserid VARCHAR2(100) NULL,
operatoruserid number(19) NULL,
buyername VARCHAR2(1000) null
);
Create Sequence aliopbulksettlementidentity
  Increment By 1
  Start With 1
  Nomaxvalue
  Nocycle
  Cache 10;
  
 Create Trigger tr_aliopbulksettlement Before Insert On aliopbulksettlement For Each Row
  Begin
  Select aliopbulksettlementidentity.Nextval Into :New.phid From dual;
  End;


CREATE TABLE aliopbulksettlementdtl(
phid number(19)  PRIMARY KEY,
appkey VARCHAR2(100) NOT NULL,
appsecret VARCHAR2(100) NOT NULL,
apptoken VARCHAR2(100) NOT NULL,
receiveid VARCHAR2(100) NULL,
id VARCHAR2(100) NULL,
status VARCHAR2(100) NULL,
orderid VARCHAR2(100) NULL,
orderentryid VARCHAR2(100) NULL,
ordersourcetype VARCHAR2(100) NULL,
qty NUMERIC(18,6) NULL,
price number(19) NULL,
originalprice number(19) NULL,
postfee number(19) NULL,
actualpayfee number(19) NULL,
productcode VARCHAR2(100) NULL,
productname VARCHAR2(1000) NULL,
sourceid VARCHAR2(100) NULL
);
Create Sequence aliopbulksettlementdtlidentity
  Increment By 1
  Start With 1
  Nomaxvalue
  Nocycle
  Cache 10;
  
 Create Trigger tr_aliopbulksettlementdtl Before Insert On aliopbulksettlementdtl For Each Row
  Begin
  Select aliopbulksettlementdtlidentity.Nextval Into :New.phid From dual;
  End;

CREATE TABLE alisupplyer(
phid number(19)  PRIMARY KEY,
appkey VARCHAR2(100) NOT NULL,
appsecret VARCHAR2(100) NOT NULL,
apptoken VARCHAR2(100) NOT NULL,
memberid VARCHAR2(100) NOT NULL,
loginid VARCHAR2(200) NOT NULL,
contact_gender VARCHAR2(100) NULL,
contact_phone VARCHAR2(100) NULL,
contact_name VARCHAR2(100) NULL,
contact_address VARCHAR2(1000) NULL,
contact_fax VARCHAR2(100) NULL,
bankaccount VARCHAR2(100) NULL,
principal VARCHAR2(100) NULL,
bank VARCHAR2(200) NULL,
registeredcapital VARCHAR2(100) NULL,
companyaddress VARCHAR2(100) NULL,
registrationid VARCHAR2(100) NULL,
dateofestablishment VARCHAR2(100) NULL,
companyname VARCHAR2(200) NULL,
businessscope VARCHAR2(100) NULL,
businessterm VARCHAR2(100) NULL,
redistrationauthority VARCHAR2(200) NULL,
enterprisetype VARCHAR2(200) NULL,
companysummary VARCHAR2(2000) NULL,
governmentinvitedsupplier INT NULL,
employeescount VARCHAR2(100) NULL,
bindalipay INT NULL,
vatinvoice INT NULL,
alipaytype VARCHAR2(100) NULL,
productionservice VARCHAR2(1000) NULL,
mainIndustries VARCHAR2(1000) NULL,
businessModel VARCHAR2(100) NULL,
businessAddress VARCHAR2(1000) NULL
);

Create Sequence alisupplyeridentity
  Increment By 1
  Start With 1
  Nomaxvalue
  Nocycle
  Cache 10;
  
 Create Trigger tr_alisupplyer Before Insert On alisupplyer For Each Row
  Begin
  Select alisupplyeridentity.Nextval Into :New.phid From dual;
  End;
  
CREATE TABLE alicertification(
phid number(19) PRIMARY KEY,
appkey VARCHAR2(100) NOT NULL,
appsecret VARCHAR2(100) NOT NULL,
apptoken VARCHAR2(100) NOT NULL,
memberid VARCHAR2(100) NOT NULL,
expiryDate DATE NULL,
code VARCHAR2(100) NULL,
authority VARCHAR2(100) NULL,
name VARCHAR2(1000) NULL,
effectivedate DATE NULL
);

Create Sequence alicertificationidentity
  Increment By 1
  Start With 1
  Nomaxvalue
  Nocycle
  Cache 10;
  
 Create Trigger tr_alicertification Before Insert On alicertification For Each Row
  Begin
  Select alicertificationidentity.Nextval Into :New.phid From dual;
  End;
  

  