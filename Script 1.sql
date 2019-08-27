CREATE TABLE alibuyoffermst(
phid BIGINT IDENTITY PRIMARY KEY,
appkey NVARCHAR(100) NOT NULL,
appsecret NVARCHAR(100) NOT NULL,
apptoken NVARCHAR(100) NOT NULL,
  offerid NVARCHAR(100) NULL,
          subject NVARCHAR(1000) NULL,
          phone NVARCHAR(100) NULL,
          contact NVARCHAR(1000) NULL,
         description  NVARCHAR(1000) NULL,
          gmtQuotationExpire DateTime NULL,
          subBizType NVARCHAR(100) NULL,
          processTemplateCode NVARCHAR(100) NULL,
          transToolType NVARCHAR(100) NULL,
          invoiceRequirement NVARCHAR(100) NULL,
          visibleAfterEndQuote int NULL,
          sourceMethodType NVARCHAR(100) NULL,
         supplierMemberIds NVARCHAR(1000) NULL,
          includeTax int NULL,
          quoteHasPostFee int NULL,
         allowPartOffer int NULL,
        certificateIds NVARCHAR(1000) NULL,
       otherCertificateNames NVARCHAR(2000) NULL,
         subuserid NVARCHAR(100) NULL,
         ngid NVARCHAR(100) NULL,
         closereason NVARCHAR(1000) NULL,
        closedesc  NVARCHAR(1000) NULL,
        receiveStreetAddress NVARCHAR(1000) NULL

)


CREATE TABLE alibuyofferdtl(
phid BIGINT IDENTITY PRIMARY KEY,
appkey NVARCHAR(100) NOT NULL,
appsecret NVARCHAR(100) NOT NULL,
apptoken NVARCHAR(100) NOT NULL,
  offerid NVARCHAR(100) NULL,
           pritemid NVARCHAR(100) NULL,
       productCode NVARCHAR(100) NULL,
         purchaseAmount NUMERIC(18,6) NULL,
         subject NVARCHAR(1000) NULL,
        descript NVARCHAR(2000) NULL,
         unit NVARCHAR(100) NULL,
         brandName NVARCHAR(1000) NULL,
         modelNumber NVARCHAR(1000) NULL

)

ALTER TABLE alibuyofferdtl ADD purchaseNoteItemId nVARCHAR(100) NULL
ALTER TABLE alibuyoffermst ADD status nVARCHAR(100) NULL
ALTER TABLE alibuyoffermst ADD gmtcreate DATETIME NULL


CREATE TABLE quotation(
phid BIGINT IDENTITY PRIMARY KEY,
appkey NVARCHAR(100) NOT NULL,
appsecret NVARCHAR(100) NOT NULL,
apptoken NVARCHAR(100) NOT NULL,
quoteid NVARCHAR(100) NULL,
ngid NVARCHAR(100) NULL,
status nVARCHAR(100) NULL,
gmtcreate DATETIME NULL,
expiredate DATETIME NULL,
buyofferid NVARCHAR(100) NULL,
suppliermemberid NVARCHAR(100) NULL,
contact NVARCHAR(100) NULL,
mobile NVARCHAR(100) NULL,
invoicetype NVARCHAR(100) NULL,
freight BIGINT NULL,
totalprice BIGINT NULL,
paytype NVARCHAR(100) NULL
)

CREATE TABLE quotationdtl(
phid BIGINT IDENTITY PRIMARY KEY,
appkey NVARCHAR(100) NOT NULL,
appsecret NVARCHAR(100) NOT NULL,
apptoken NVARCHAR(100) NOT NULL,
quoteid NVARCHAR(100) NULL,
quotedid NVARCHAR(100) NULL,
pritemid NVARCHAR(100) NULL,
       productcode NVARCHAR(100) NULL,
         amount NUMERIC(18,6) NULL,
         price BIGINT NULL,
         subject NVARCHAR(1000) NULL,
        itemcount NUMERIC(18,6) NULL,
         unit NVARCHAR(100) NULL,
         brandname NVARCHAR(1000) NULL,
         modelnumber NVARCHAR(1000) NULL,
         taxrate NVARCHAR(100) NULL
         
)

CREATE TABLE aliordermst(
phid BIGINT IDENTITY PRIMARY KEY,
appkey NVARCHAR(100) NOT NULL,
appsecret NVARCHAR(100) NOT NULL,
apptoken NVARCHAR(100) NOT NULL,
orderid NVARCHAR(100) NULL,
status  NVARCHAR(100) NULL,
subbiz NVARCHAR(100) NULL,
gmtcreate DATETIME NULL,
discount BIGINT NULL,
carriage BIGINT NULL,
refundpayment BIGINT NULL,
sumpayment BIGINT NULL,
sumproductpayment BIGINT NULL,
sellercompanyname NVARCHAR(1000) NULL,
selleremail NVARCHAR(1000) NULL,
sellermemberid NVARCHAR(100) NULL,
selleruserid BIGINT NULL,
selleralipayid NVARCHAR(100) NULL,
sellerloginid NVARCHAR(100) NULL,
buyercompanyname NVARCHAR(1000) NULL,
buyeremail NVARCHAR(1000) NULL,
buyermemberid NVARCHAR(100) NULL,
buyeruserid BIGINT NULL,
buyeralipayid NVARCHAR(100) NULL,
buyerloginid NVARCHAR(100) NULL,
)

CREATE TABLE aliorderdtl(
phid BIGINT IDENTITY PRIMARY KEY,
appkey NVARCHAR(100) NOT NULL,
appsecret NVARCHAR(100) NOT NULL,
apptoken NVARCHAR(100) NOT NULL,
orderid NVARCHAR(100) NULL,
orderdtlid NVARCHAR(100) NULL,
productName NVARCHAR(1000) NULL,
unit NVARCHAR(100) NULL,
qty NUMERIC(18,6) NULL,
price BIGINT  NULL,
amt BIGINT  NULL,
refundFee BIGINT NULL,
discount NUMERIC(18,6) NULL,
sourceid NVARCHAR(100) NULL,
status NVARCHAR(100) NULL,
actualpayfee BIGINT NULL,
)

ALTER TABLE aliordermst ADD sellername nVARCHAR(100) NULL
ALTER TABLE aliordermst ADD buyername nVARCHAR(100) NULL



CREATE TABLE aliopbulksettlement(
phid BIGINT IDENTITY PRIMARY KEY,
appkey NVARCHAR(100) NOT NULL,
appsecret NVARCHAR(100) NOT NULL,
apptoken NVARCHAR(100) NOT NULL,
receiveid NVARCHAR(100) NULL,
status NVARCHAR(100) NULL,
actualpayfee BIGINT NULL,
issuccess INT NULL,
orderid NVARCHAR(100) NULL,
gmtcreate DATETIME NULL,
gmtreceive DATETIME NULL,
selleruserid BIGINT NULL,
sellercompanyname NVARCHAR(1000) NULL,
operatorusername NVARCHAR(1000) NULL,
buyeruserid NVARCHAR(100) NULL,
operatoruserid BIGINT NULL,
buyername NVARCHAR(1000) null
)



CREATE TABLE aliopbulksettlementdtl(
phid BIGINT IDENTITY PRIMARY KEY,
appkey NVARCHAR(100) NOT NULL,
appsecret NVARCHAR(100) NOT NULL,
apptoken NVARCHAR(100) NOT NULL,
receiveid NVARCHAR(100) NULL,
id NVARCHAR(100) NULL,
status NVARCHAR(100) NULL,
orderid NVARCHAR(100) NULL,
orderentryid NVARCHAR(100) NULL,
ordersourcetype NVARCHAR(100) NULL,
qty NUMERIC(18,6) NULL,
price BIGINT NULL,
originalprice BIGINT NULL,
postfee BIGINT NULL,
actualpayfee BIGINT NULL,
productcode NVARCHAR(100) NULL,
productname nVARCHAR(1000) NULL,
sourceid nVARCHAR(100) NULL
)


SELECT * FROM aliopbulksettlement
SELECT * FROM aliopbulksettlementdtl

CREATE TABLE alisupplyer(
phid BIGINT IDENTITY PRIMARY KEY,
appkey NVARCHAR(100) NOT NULL,
appsecret NVARCHAR(100) NOT NULL,
apptoken NVARCHAR(100) NOT NULL,
memberid NVARCHAR(100) NOT NULL,
loginid NVARCHAR(200) NOT NULL,
contact_gender NVARCHAR(100) NULL,
contact_phone NVARCHAR(100) NULL,
contact_name NVARCHAR(100) NULL,
contact_address NVARCHAR(1000) NULL,
contact_fax NVARCHAR(100) NULL,
bankaccount NVARCHAR(100) NULL,
principal NVARCHAR(100) NULL,
bank NVARCHAR(200) NULL,
registeredcapital NVARCHAR(100) NULL,
companyaddress NVARCHAR(100) NULL,
registrationid NVARCHAR(100) NULL,
dateofestablishment NVARCHAR(100) NULL,
companyname NVARCHAR(200) NULL,
businessscope NVARCHAR(100) NULL,
businessterm NVARCHAR(100) NULL,
redistrationauthority NVARCHAR(200) NULL,
enterprisetype NVARCHAR(200) NULL,
companysummary NVARCHAR(2000) NULL,
governmentinvitedsupplier INT NULL,
employeescount NVARCHAR(100) NULL,
bindalipay INT NULL,
vatinvoice INT NULL,
alipaytype NVARCHAR(100) NULL,
productionservice NVARCHAR(1000) NULL,
mainIndustries NVARCHAR(1000) NULL,
businessModel NVARCHAR(100) NULL,
businessAddress NVARCHAR(1000) NULL
)

CREATE TABLE alicertification(
phid BIGINT IDENTITY PRIMARY KEY,
appkey NVARCHAR(100) NOT NULL,
appsecret NVARCHAR(100) NOT NULL,
apptoken NVARCHAR(100) NOT NULL,
memberid NVARCHAR(100) NOT NULL,
expiryDate DATETIME NULL,
code NVARCHAR(100) NULL,
authority NVARCHAR(100) NULL,
name NVARCHAR(1000) NULL,
effectivedate DATETIME NULL
)