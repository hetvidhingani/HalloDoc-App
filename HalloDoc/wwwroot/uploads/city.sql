PGDMP                      |            HalloDoc    16.1    16.1    j           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            k           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            l           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            m           1262    32768    HalloDoc    DATABASE     }   CREATE DATABASE "HalloDoc" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_India.1252';
    DROP DATABASE "HalloDoc";
                postgres    false            �            1259    32855    Admin    TABLE     �  CREATE TABLE public."Admin" (
    "AdminId" integer NOT NULL,
    "AspNetUserId" character varying(128) NOT NULL,
    "FirstName" character varying(100) NOT NULL,
    "LastName" character varying(100),
    "Email" character varying(50) NOT NULL,
    "Mobile" character varying(20),
    "Address1" character varying(500),
    "Address2" character varying(500),
    "City" character varying(100),
    "RegionId" integer,
    "Zip" character varying(10),
    "AltPhone" character varying(20),
    "CreatedBy" character varying(128) NOT NULL,
    "CreatedDate" timestamp without time zone NOT NULL,
    "ModifiedBy" character varying(128),
    "ModifiedDate" timestamp without time zone,
    "Status" smallint,
    "IsDeleted" boolean,
    "RoleId" integer
);
    DROP TABLE public."Admin";
       public         heap    postgres    false            �            1259    32924    AdminRegion    TABLE     �   CREATE TABLE public."AdminRegion" (
    "AdminRegionId" integer NOT NULL,
    "AdminId" integer NOT NULL,
    "RegionId" integer NOT NULL
);
 !   DROP TABLE public."AdminRegion";
       public         heap    postgres    false            �            1259    32923    AdminRegion_AdminRegionId_seq    SEQUENCE     �   CREATE SEQUENCE public."AdminRegion_AdminRegionId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 6   DROP SEQUENCE public."AdminRegion_AdminRegionId_seq";
       public          postgres    false    229            n           0    0    AdminRegion_AdminRegionId_seq    SEQUENCE OWNED BY     e   ALTER SEQUENCE public."AdminRegion_AdminRegionId_seq" OWNED BY public."AdminRegion"."AdminRegionId";
          public          postgres    false    228            �            1259    32854    Admin_AdminId_seq    SEQUENCE     �   CREATE SEQUENCE public."Admin_AdminId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public."Admin_AdminId_seq";
       public          postgres    false    218            o           0    0    Admin_AdminId_seq    SEQUENCE OWNED BY     M   ALTER SEQUENCE public."Admin_AdminId_seq" OWNED BY public."Admin"."AdminId";
          public          postgres    false    217            �            1259    32849    AspNetRoles    TABLE     |   CREATE TABLE public."AspNetRoles" (
    "Id" character varying(128) NOT NULL,
    "Name" character varying(256) NOT NULL
);
 !   DROP TABLE public."AspNetRoles";
       public         heap    postgres    false            �            1259    32878    AspNetUserRoles    TABLE     �   CREATE TABLE public."AspNetUserRoles" (
    "UserId" character varying(128) NOT NULL,
    "RoleId" character varying(128) NOT NULL
);
 %   DROP TABLE public."AspNetUserRoles";
       public         heap    postgres    false            �            1259    32842    AspNetUsers    TABLE     w  CREATE TABLE public."AspNetUsers" (
    "Id" character varying(128) NOT NULL,
    "UserName" character varying(256) NOT NULL,
    "PasswordHash" character varying,
    "Email" character varying(256),
    "PhoneNumber" character varying,
    "IP" character varying(20),
    "CreatedDate" timestamp without time zone NOT NULL,
    "ModifiedDate" timestamp without time zone
);
 !   DROP TABLE public."AspNetUsers";
       public         heap    postgres    false            �            1259    32894    BlockRequests    TABLE     �  CREATE TABLE public."BlockRequests" (
    "BlockRequestId" integer NOT NULL,
    "PhoneNumber" character varying(50),
    "Email" character varying(50),
    "IsActive" bit(1),
    "Reason" character varying,
    "RequestId" character varying(50) NOT NULL,
    "IP" character varying(20),
    "CreatedDate" timestamp without time zone,
    "ModifiedDate" timestamp without time zone
);
 #   DROP TABLE public."BlockRequests";
       public         heap    postgres    false            �            1259    32893     BlockRequests_BlockRequestId_seq    SEQUENCE     �   CREATE SEQUENCE public."BlockRequests_BlockRequestId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 9   DROP SEQUENCE public."BlockRequests_BlockRequestId_seq";
       public          postgres    false    221            p           0    0     BlockRequests_BlockRequestId_seq    SEQUENCE OWNED BY     k   ALTER SEQUENCE public."BlockRequests_BlockRequestId_seq" OWNED BY public."BlockRequests"."BlockRequestId";
          public          postgres    false    220            �            1259    32941    Business    TABLE     �  CREATE TABLE public."Business" (
    "BusinessId" integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "BusinessTypeId" integer,
    "Address1" character varying(500),
    "Address2" character varying(500),
    "City" character varying(50),
    "RegionId" integer,
    "ZipCode" character varying(10),
    "PhoneNumber" character varying(20),
    "FaxNumber" character varying(20),
    "IsRegistered" bit(1),
    "CreatedBy" character varying(128),
    "CreatedDate" timestamp without time zone NOT NULL,
    "ModifiedBy" character varying(128),
    "ModifiedDate" timestamp without time zone,
    "Status" smallint,
    "IsDeleted" bit(1),
    "IP" character varying(20)
);
    DROP TABLE public."Business";
       public         heap    postgres    false            �            1259    32903    BusinessType    TABLE     y   CREATE TABLE public."BusinessType" (
    "BusinessTypeId" integer NOT NULL,
    "Name" character varying(50) NOT NULL
);
 "   DROP TABLE public."BusinessType";
       public         heap    postgres    false            �            1259    32902    BusinessType_BusinessTypeId_seq    SEQUENCE     �   CREATE SEQUENCE public."BusinessType_BusinessTypeId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 8   DROP SEQUENCE public."BusinessType_BusinessTypeId_seq";
       public          postgres    false    223            q           0    0    BusinessType_BusinessTypeId_seq    SEQUENCE OWNED BY     i   ALTER SEQUENCE public."BusinessType_BusinessTypeId_seq" OWNED BY public."BusinessType"."BusinessTypeId";
          public          postgres    false    222            �            1259    32940    Business_BusinessId_seq    SEQUENCE     �   CREATE SEQUENCE public."Business_BusinessId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 0   DROP SEQUENCE public."Business_BusinessId_seq";
       public          postgres    false    231            r           0    0    Business_BusinessId_seq    SEQUENCE OWNED BY     Y   ALTER SEQUENCE public."Business_BusinessId_seq" OWNED BY public."Business"."BusinessId";
          public          postgres    false    230            �            1259    32910    CaseTag    TABLE     o   CREATE TABLE public."CaseTag" (
    "CaseTagId" integer NOT NULL,
    "Name" character varying(50) NOT NULL
);
    DROP TABLE public."CaseTag";
       public         heap    postgres    false            �            1259    32909    CaseTag_CaseTagId_seq    SEQUENCE     �   CREATE SEQUENCE public."CaseTag_CaseTagId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 .   DROP SEQUENCE public."CaseTag_CaseTagId_seq";
       public          postgres    false    225            s           0    0    CaseTag_CaseTagId_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public."CaseTag_CaseTagId_seq" OWNED BY public."CaseTag"."CaseTagId";
          public          postgres    false    224            �            1259    32971 	   Concierge    TABLE     �  CREATE TABLE public."Concierge" (
    "ConciergeId" integer NOT NULL,
    "ConciergeName" character varying(100) NOT NULL,
    "Address" character varying(150),
    "Street" character varying(50) NOT NULL,
    "City" character varying(50) NOT NULL,
    "State" character varying(50) NOT NULL,
    "ZipCode" character varying(50) NOT NULL,
    "CreatedDate" timestamp without time zone NOT NULL,
    "RegionId" integer,
    "IP" character varying(20)
);
    DROP TABLE public."Concierge";
       public         heap    postgres    false            �            1259    32970    Concierge_ConciergeId_seq    SEQUENCE     �   CREATE SEQUENCE public."Concierge_ConciergeId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 2   DROP SEQUENCE public."Concierge_ConciergeId_seq";
       public          postgres    false    233            t           0    0    Concierge_ConciergeId_seq    SEQUENCE OWNED BY     ]   ALTER SEQUENCE public."Concierge_ConciergeId_seq" OWNED BY public."Concierge"."ConciergeId";
          public          postgres    false    232            �            1259    33004    EmailLog    TABLE     4  CREATE TABLE public."EmailLog" (
    "EmailLogID" numeric(9,0) NOT NULL,
    "EmailTemplate" character varying,
    "SubjectName" character varying(200) NOT NULL,
    "EmailID" character varying(200) NOT NULL,
    "ConfirmationNumber" character varying(200),
    "FilePath" character varying,
    "RoleId" integer,
    "RequestId" integer,
    "AdminId" integer,
    "PhysicianId" integer,
    "CreateDate" timestamp without time zone NOT NULL,
    "SentDate" timestamp without time zone,
    "IsEmailSent" bit(1),
    "SentTries" integer,
    "Action" integer
);
    DROP TABLE public."EmailLog";
       public         heap    postgres    false            �            1259    33012    HealthProfessionalType    TABLE     �   CREATE TABLE public."HealthProfessionalType" (
    "HealthProfessionalId" integer NOT NULL,
    "ProfessionName" character varying(50) NOT NULL,
    "CreatedDate" timestamp without time zone NOT NULL,
    "IsActive" bit(1),
    "IsDeleted" bit(1)
);
 ,   DROP TABLE public."HealthProfessionalType";
       public         heap    postgres    false            �            1259    33011 /   HealthProfessionalType_HealthProfessionalId_seq    SEQUENCE     �   CREATE SEQUENCE public."HealthProfessionalType_HealthProfessionalId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 H   DROP SEQUENCE public."HealthProfessionalType_HealthProfessionalId_seq";
       public          postgres    false    237            u           0    0 /   HealthProfessionalType_HealthProfessionalId_seq    SEQUENCE OWNED BY     �   ALTER SEQUENCE public."HealthProfessionalType_HealthProfessionalId_seq" OWNED BY public."HealthProfessionalType"."HealthProfessionalId";
          public          postgres    false    236            �            1259    33019    HealthProfessionals    TABLE     �  CREATE TABLE public."HealthProfessionals" (
    "VendorId" integer NOT NULL,
    "VendorName" character varying(100) NOT NULL,
    "Profession" integer,
    "FaxNumber" character varying(50) NOT NULL,
    "Address" character varying(150),
    "City" character varying(100),
    "State" character varying(50),
    "Zip" character varying(50),
    "RegionId" integer,
    "CreatedDate" timestamp without time zone NOT NULL,
    "ModifiedDate" timestamp without time zone,
    "PhoneNumber" character varying(100),
    "IsDeleted" bit(1),
    "IP" character varying(20),
    "Email" character varying(50),
    "BusinessContact" character varying(100)
);
 )   DROP TABLE public."HealthProfessionals";
       public         heap    postgres    false            �            1259    33018     HealthProfessionals_VendorId_seq    SEQUENCE     �   CREATE SEQUENCE public."HealthProfessionals_VendorId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 9   DROP SEQUENCE public."HealthProfessionals_VendorId_seq";
       public          postgres    false    239            v           0    0     HealthProfessionals_VendorId_seq    SEQUENCE OWNED BY     k   ALTER SEQUENCE public."HealthProfessionals_VendorId_seq" OWNED BY public."HealthProfessionals"."VendorId";
          public          postgres    false    238                       1259    33280    Menu    TABLE     �   CREATE TABLE public."Menu" (
    "MenuId" integer NOT NULL,
    "Name" character varying(50) NOT NULL,
    "AccountType" smallint NOT NULL,
    "SortOrder" integer,
    CONSTRAINT "Menu_AccountType_check" CHECK (("AccountType" = ANY (ARRAY[1, 2])))
);
    DROP TABLE public."Menu";
       public         heap    postgres    false                       1259    33279    Menu_MenuId_seq    SEQUENCE     �   CREATE SEQUENCE public."Menu_MenuId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public."Menu_MenuId_seq";
       public          postgres    false    269            w           0    0    Menu_MenuId_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public."Menu_MenuId_seq" OWNED BY public."Menu"."MenuId";
          public          postgres    false    268            �            1259    33033    OrderDetails    TABLE     u  CREATE TABLE public."OrderDetails" (
    "Id" integer NOT NULL,
    "VendorId" integer,
    "RequestId" integer,
    "FaxNumber" character varying(50),
    "Email" character varying(50),
    "BusinessContact" character varying(100),
    "Prescription" text,
    "NoOfRefill" integer,
    "CreatedDate" timestamp without time zone,
    "CreatedBy" character varying(100)
);
 "   DROP TABLE public."OrderDetails";
       public         heap    postgres    false            �            1259    33032    OrderDetails_Id_seq    SEQUENCE     �   CREATE SEQUENCE public."OrderDetails_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public."OrderDetails_Id_seq";
       public          postgres    false    241            x           0    0    OrderDetails_Id_seq    SEQUENCE OWNED BY     Q   ALTER SEQUENCE public."OrderDetails_Id_seq" OWNED BY public."OrderDetails"."Id";
          public          postgres    false    240            �            1259    33042 	   Physician    TABLE       CREATE TABLE public."Physician" (
    "PhysicianId" integer NOT NULL,
    "Id" character varying(128),
    "FirstName" character varying(100) NOT NULL,
    "LastName" character varying(100),
    "Email" character varying(50) NOT NULL,
    "Mobile" character varying(20),
    "MedicalLicense" character varying(500),
    "Photo" character varying(100),
    "AdminNotes" character varying(500),
    "IsAgreementDoc" bit(1),
    "IsBackgroundDoc" bit(1),
    "IsTrainingDoc" bit(1),
    "IsNonDisclosureDoc" bit(1),
    "Address1" character varying(500),
    "Address2" character varying(500),
    "City" character varying(100),
    "RegionId" integer,
    "Zip" character varying(10),
    "AltPhone" character varying(20),
    "CreatedBy" character varying(128),
    "CreatedDate" timestamp without time zone NOT NULL,
    "ModifiedBy" character varying(128),
    "ModifiedDate" timestamp without time zone,
    "Status" smallint,
    "BusinessName" character varying(100) NOT NULL,
    "BusinessWebsite" character varying(200) NOT NULL,
    "IsDeleted" bit(1),
    "RoleId" integer,
    "NPINumber" character varying(500),
    "IsLicenseDoc" bit(1),
    "Signature" character varying(100),
    "IsCredentialDoc" bit(1),
    "IsTokenGenerate" bit(1),
    "SyncEmailAddress" character varying(50)
);
    DROP TABLE public."Physician";
       public         heap    postgres    false                       1259    33364    PhysicianLocation    TABLE     .  CREATE TABLE public."PhysicianLocation" (
    "LocationId" integer NOT NULL,
    "PhysicianId" integer NOT NULL,
    "Latitude" numeric(9,6),
    "Longitude" numeric(9,6),
    "CreatedDate" timestamp without time zone,
    "PhysicianName" character varying(50),
    "Address" character varying(500)
);
 '   DROP TABLE public."PhysicianLocation";
       public         heap    postgres    false                       1259    33363     PhysicianLocation_LocationId_seq    SEQUENCE     �   CREATE SEQUENCE public."PhysicianLocation_LocationId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 9   DROP SEQUENCE public."PhysicianLocation_LocationId_seq";
       public          postgres    false    280            y           0    0     PhysicianLocation_LocationId_seq    SEQUENCE OWNED BY     k   ALTER SEQUENCE public."PhysicianLocation_LocationId_seq" OWNED BY public."PhysicianLocation"."LocationId";
          public          postgres    false    279            �            1259    33066    PhysicianNotification    TABLE     �   CREATE TABLE public."PhysicianNotification" (
    id integer NOT NULL,
    "PhysicianId" integer NOT NULL,
    "IsNotificationStopped" bit(1) NOT NULL
);
 +   DROP TABLE public."PhysicianNotification";
       public         heap    postgres    false            �            1259    33065    PhysicianNotification_id_seq    SEQUENCE     �   CREATE SEQUENCE public."PhysicianNotification_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 5   DROP SEQUENCE public."PhysicianNotification_id_seq";
       public          postgres    false    245            z           0    0    PhysicianNotification_id_seq    SEQUENCE OWNED BY     a   ALTER SEQUENCE public."PhysicianNotification_id_seq" OWNED BY public."PhysicianNotification".id;
          public          postgres    false    244            �            1259    33078    PhysicianRegion    TABLE     �   CREATE TABLE public."PhysicianRegion" (
    "PhysicianRegionId" integer NOT NULL,
    "PhysicianId" integer NOT NULL,
    "RegionId" integer NOT NULL
);
 %   DROP TABLE public."PhysicianRegion";
       public         heap    postgres    false            �            1259    33077 %   PhysicianRegion_PhysicianRegionId_seq    SEQUENCE     �   CREATE SEQUENCE public."PhysicianRegion_PhysicianRegionId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 >   DROP SEQUENCE public."PhysicianRegion_PhysicianRegionId_seq";
       public          postgres    false    247            {           0    0 %   PhysicianRegion_PhysicianRegionId_seq    SEQUENCE OWNED BY     u   ALTER SEQUENCE public."PhysicianRegion_PhysicianRegionId_seq" OWNED BY public."PhysicianRegion"."PhysicianRegionId";
          public          postgres    false    246            �            1259    33041    Physician_PhysicianId_seq    SEQUENCE     �   CREATE SEQUENCE public."Physician_PhysicianId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 2   DROP SEQUENCE public."Physician_PhysicianId_seq";
       public          postgres    false    243            |           0    0    Physician_PhysicianId_seq    SEQUENCE OWNED BY     ]   ALTER SEQUENCE public."Physician_PhysicianId_seq" OWNED BY public."Physician"."PhysicianId";
          public          postgres    false    242            �            1259    32917    Region    TABLE     �   CREATE TABLE public."Region" (
    "RegionId" integer NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Abbreviation" character varying(50)
);
    DROP TABLE public."Region";
       public         heap    postgres    false            �            1259    32916    Region_RegionId_seq    SEQUENCE     �   CREATE SEQUENCE public."Region_RegionId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 ,   DROP SEQUENCE public."Region_RegionId_seq";
       public          postgres    false    227            }           0    0    Region_RegionId_seq    SEQUENCE OWNED BY     Q   ALTER SEQUENCE public."Region_RegionId_seq" OWNED BY public."Region"."RegionId";
          public          postgres    false    226            �            1259    33095    Request    TABLE     ,  CREATE TABLE public."Request" (
    "RequestId" integer NOT NULL,
    "RequestTypeId" integer NOT NULL,
    "UserId" integer,
    "FirstName" character varying(100),
    "LastName" character varying(100),
    "PhoneNumber" character varying(23),
    "Email" character varying(50),
    "Status" smallint NOT NULL,
    "PhysicianId" integer,
    "ConfirmationNumber" character varying(20),
    "CreatedDate" timestamp without time zone NOT NULL,
    "IsDeleted" bit(1),
    "ModifiedDate" timestamp without time zone,
    "DeclinedBy" character varying(250),
    "IsUrgentEmailSent" bit(1),
    "LastWellnessDate" timestamp without time zone,
    "IsMobile" bit(1),
    "CallType" smallint,
    "CompletedByPhysician" bit(1),
    "LastReservationDate" timestamp without time zone,
    "AcceptedDate" timestamp without time zone,
    "RelationName" character varying(100),
    "CaseNumber" character varying(50),
    "IP" character varying(20),
    "CaseTag" character varying(50),
    "CaseTagPhysician" character varying(50),
    "PatientAccountId" character varying(128),
    "CreatedUserId" integer,
    CONSTRAINT "Request_RequestTypeId_check" CHECK (("RequestTypeId" = ANY (ARRAY[1, 2, 3, 4]))),
    CONSTRAINT "Request_Status_check" CHECK (("Status" = ANY (ARRAY[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15])))
);
    DROP TABLE public."Request";
       public         heap    postgres    false            �            1259    33116    RequestBusiness    TABLE     �   CREATE TABLE public."RequestBusiness" (
    "RequestBusinessId" integer NOT NULL,
    "RequestId" integer NOT NULL,
    "BusinessId" integer NOT NULL,
    "IP" character varying(20)
);
 %   DROP TABLE public."RequestBusiness";
       public         heap    postgres    false            �            1259    33115 %   RequestBusiness_RequestBusinessId_seq    SEQUENCE     �   CREATE SEQUENCE public."RequestBusiness_RequestBusinessId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 >   DROP SEQUENCE public."RequestBusiness_RequestBusinessId_seq";
       public          postgres    false    251            ~           0    0 %   RequestBusiness_RequestBusinessId_seq    SEQUENCE OWNED BY     u   ALTER SEQUENCE public."RequestBusiness_RequestBusinessId_seq" OWNED BY public."RequestBusiness"."RequestBusinessId";
          public          postgres    false    250            �            1259    33133    RequestClient    TABLE       CREATE TABLE public."RequestClient" (
    "RequestClientId" integer NOT NULL,
    "RequestId" integer NOT NULL,
    "FirstName" character varying(100) NOT NULL,
    "LastName" character varying(100),
    "PhoneNumber" character varying(23),
    "Location" character varying(100),
    "Address" character varying(500),
    "RegionId" integer,
    "NotiMobile" character varying(20),
    "NotiEmail" character varying(50),
    "Notes" character varying(500),
    "Email" character varying(50),
    "strMonth" character varying(20),
    "intYear" integer,
    "intDate" integer,
    "IsMobile" bit(1),
    "Street" character varying(100),
    "City" character varying(100),
    "State" character varying(100),
    "ZipCode" character varying(10),
    "CommunicationType" smallint,
    "RemindReservationCount" smallint,
    "RemindHouseCallCount" smallint,
    "IsSetFollowupSent" smallint,
    "IP" character varying(20),
    "IsReservationReminderSent" smallint,
    "Latitude" numeric(9,6),
    "Longitude" numeric(9,6),
    "DateOfBirth" date
);
 #   DROP TABLE public."RequestClient";
       public         heap    postgres    false            �            1259    33132 !   RequestClient_RequestClientId_seq    SEQUENCE     �   CREATE SEQUENCE public."RequestClient_RequestClientId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 :   DROP SEQUENCE public."RequestClient_RequestClientId_seq";
       public          postgres    false    253                       0    0 !   RequestClient_RequestClientId_seq    SEQUENCE OWNED BY     m   ALTER SEQUENCE public."RequestClient_RequestClientId_seq" OWNED BY public."RequestClient"."RequestClientId";
          public          postgres    false    252                       1259    33181    RequestClosed    TABLE       CREATE TABLE public."RequestClosed" (
    "RequestClosedId" integer NOT NULL,
    "RequestId" integer NOT NULL,
    "RequestStatusLogId" integer NOT NULL,
    "PhyNotes" character varying(500),
    "ClientNotes" character varying(500),
    "IP" character varying(20)
);
 #   DROP TABLE public."RequestClosed";
       public         heap    postgres    false                        1259    33180 !   RequestClosed_RequestClosedId_seq    SEQUENCE     �   CREATE SEQUENCE public."RequestClosed_RequestClosedId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 :   DROP SEQUENCE public."RequestClosed_RequestClosedId_seq";
       public          postgres    false    257            �           0    0 !   RequestClosed_RequestClosedId_seq    SEQUENCE OWNED BY     m   ALTER SEQUENCE public."RequestClosed_RequestClosedId_seq" OWNED BY public."RequestClosed"."RequestClosedId";
          public          postgres    false    256                       1259    33200    RequestConcierge    TABLE     �   CREATE TABLE public."RequestConcierge" (
    "Id" integer NOT NULL,
    "RequestId" integer NOT NULL,
    "ConciergeId" integer NOT NULL,
    "IP" character varying(20)
);
 &   DROP TABLE public."RequestConcierge";
       public         heap    postgres    false                       1259    33199    RequestConcierge_Id_seq    SEQUENCE     �   CREATE SEQUENCE public."RequestConcierge_Id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 0   DROP SEQUENCE public."RequestConcierge_Id_seq";
       public          postgres    false    259            �           0    0    RequestConcierge_Id_seq    SEQUENCE OWNED BY     Y   ALTER SEQUENCE public."RequestConcierge_Id_seq" OWNED BY public."RequestConcierge"."Id";
          public          postgres    false    258                       1259    33217    RequestNotes    TABLE     %  CREATE TABLE public."RequestNotes" (
    "RequestNotesId" integer NOT NULL,
    "RequestId" integer NOT NULL,
    "strMonth" character varying(20),
    "intYear" integer,
    "intDate" integer,
    "PhysicianNotes" character varying(500),
    "AdminNotes" character varying(500),
    "CreatedBy" character varying(128),
    "CreatedDate" timestamp without time zone NOT NULL,
    "ModifiedBy" character varying(128),
    "ModifiedDate" timestamp without time zone,
    "IP" character varying(20),
    "AdministrativeNotes" character varying(500)
);
 "   DROP TABLE public."RequestNotes";
       public         heap    postgres    false                       1259    33216    RequestNotes_RequestNotesId_seq    SEQUENCE     �   CREATE SEQUENCE public."RequestNotes_RequestNotesId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 8   DROP SEQUENCE public."RequestNotes_RequestNotesId_seq";
       public          postgres    false    261            �           0    0    RequestNotes_RequestNotesId_seq    SEQUENCE OWNED BY     i   ALTER SEQUENCE public."RequestNotes_RequestNotesId_seq" OWNED BY public."RequestNotes"."RequestNotesId";
          public          postgres    false    260                       1259    49157     RequestNotes_RequestNotesId_seq1    SEQUENCE     �   ALTER TABLE public."RequestNotes" ALTER COLUMN "RequestNotesId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."RequestNotes_RequestNotesId_seq1"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    261            �            1259    33152    RequestStatusLog    TABLE     �  CREATE TABLE public."RequestStatusLog" (
    "RequestStatusLogId" integer NOT NULL,
    "RequestId" integer NOT NULL,
    "Status" smallint NOT NULL,
    "PhysicianId" integer,
    "AdminId" integer,
    "TransToPhysicianId" integer,
    "Notes" character varying(500),
    "CreatedDate" timestamp without time zone NOT NULL,
    "IP" character varying(20),
    "TransToAdmin" bit(1)
);
 &   DROP TABLE public."RequestStatusLog";
       public         heap    postgres    false            �            1259    33151 '   RequestStatusLog_RequestStatusLogId_seq    SEQUENCE     �   CREATE SEQUENCE public."RequestStatusLog_RequestStatusLogId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 @   DROP SEQUENCE public."RequestStatusLog_RequestStatusLogId_seq";
       public          postgres    false    255            �           0    0 '   RequestStatusLog_RequestStatusLogId_seq    SEQUENCE OWNED BY     y   ALTER SEQUENCE public."RequestStatusLog_RequestStatusLogId_seq" OWNED BY public."RequestStatusLog"."RequestStatusLogId";
          public          postgres    false    254                       1259    33241    RequestType    TABLE     w   CREATE TABLE public."RequestType" (
    "RequestTypeId" integer NOT NULL,
    "Name" character varying(50) NOT NULL
);
 !   DROP TABLE public."RequestType";
       public         heap    postgres    false                       1259    33240    RequestType_RequestTypeId_seq    SEQUENCE     �   CREATE SEQUENCE public."RequestType_RequestTypeId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 6   DROP SEQUENCE public."RequestType_RequestTypeId_seq";
       public          postgres    false    263            �           0    0    RequestType_RequestTypeId_seq    SEQUENCE OWNED BY     e   ALTER SEQUENCE public."RequestType_RequestTypeId_seq" OWNED BY public."RequestType"."RequestTypeId";
          public          postgres    false    262            	           1259    33248    RequestWiseFile    TABLE     �  CREATE TABLE public."RequestWiseFile" (
    "RequestWiseFileID" integer NOT NULL,
    "RequestId" integer NOT NULL,
    "FileName" character varying(500),
    "CreatedDate" timestamp without time zone NOT NULL,
    "PhysicianId" integer,
    "AdminId" integer,
    "DocType" smallint,
    "IsFrontSide" bit(1),
    "IsCompensation" bit(1),
    "IP" character varying(20),
    "IsFinalize" bit(1),
    "IsDeleted" bit(1),
    "IsPatientRecords" bit(1)
);
 %   DROP TABLE public."RequestWiseFile";
       public         heap    postgres    false                       1259    33247 %   RequestWiseFile_RequestWiseFileID_seq    SEQUENCE     �   CREATE SEQUENCE public."RequestWiseFile_RequestWiseFileID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 >   DROP SEQUENCE public."RequestWiseFile_RequestWiseFileID_seq";
       public          postgres    false    265            �           0    0 %   RequestWiseFile_RequestWiseFileID_seq    SEQUENCE OWNED BY     u   ALTER SEQUENCE public."RequestWiseFile_RequestWiseFileID_seq" OWNED BY public."RequestWiseFile"."RequestWiseFileID";
          public          postgres    false    264            �            1259    33094    Request_RequestId_seq    SEQUENCE     �   CREATE SEQUENCE public."Request_RequestId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 .   DROP SEQUENCE public."Request_RequestId_seq";
       public          postgres    false    249            �           0    0    Request_RequestId_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public."Request_RequestId_seq" OWNED BY public."Request"."RequestId";
          public          postgres    false    248                       1259    40978    Request_RequestId_seq1    SEQUENCE     �   ALTER TABLE public."Request" ALTER COLUMN "RequestId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Request_RequestId_seq1"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    249                       1259    33272    Role    TABLE     �  CREATE TABLE public."Role" (
    "RoleId" integer NOT NULL,
    "Name" character varying(50) NOT NULL,
    "AccountType" smallint NOT NULL,
    "CreatedBy" character varying(128) NOT NULL,
    "CreatedDate" timestamp without time zone NOT NULL,
    "ModifiedBy" character varying(128),
    "ModifiedDate" timestamp without time zone,
    "IsDeleted" bit(1) NOT NULL,
    "IP" character varying(20),
    CONSTRAINT "Role_AccountType_check" CHECK (("AccountType" = ANY (ARRAY[1, 2])))
);
    DROP TABLE public."Role";
       public         heap    postgres    false                       1259    33288    RoleMenu    TABLE     �   CREATE TABLE public."RoleMenu" (
    "RoleMenuId" integer NOT NULL,
    "RoleId" integer NOT NULL,
    "MenuId" integer NOT NULL
);
    DROP TABLE public."RoleMenu";
       public         heap    postgres    false                       1259    33287    RoleMenu_RoleMenuId_seq    SEQUENCE     �   CREATE SEQUENCE public."RoleMenu_RoleMenuId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 0   DROP SEQUENCE public."RoleMenu_RoleMenuId_seq";
       public          postgres    false    271            �           0    0    RoleMenu_RoleMenuId_seq    SEQUENCE OWNED BY     Y   ALTER SEQUENCE public."RoleMenu_RoleMenuId_seq" OWNED BY public."RoleMenu"."RoleMenuId";
          public          postgres    false    270            
           1259    33271    Role_RoleId_seq    SEQUENCE     �   CREATE SEQUENCE public."Role_RoleId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public."Role_RoleId_seq";
       public          postgres    false    267            �           0    0    Role_RoleId_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public."Role_RoleId_seq" OWNED BY public."Role"."RoleId";
          public          postgres    false    266                       1259    33356    SMSLog    TABLE     �  CREATE TABLE public."SMSLog" (
    "SMSLogID" numeric(9,0) NOT NULL,
    "SMSTemplate" character varying NOT NULL,
    "MobileNumber" character varying(50) NOT NULL,
    "ConfirmationNumber" character varying(200),
    "RoleId" integer,
    "AdminId" integer,
    "RequestId" integer,
    "PhysicianId" integer,
    "CreateDate" timestamp without time zone NOT NULL,
    "SentDate" timestamp without time zone,
    "IsSMSSent" bit(1),
    "SentTries" integer NOT NULL,
    "Action" integer
);
    DROP TABLE public."SMSLog";
       public         heap    postgres    false                       1259    33305    Shift    TABLE     c  CREATE TABLE public."Shift" (
    "ShiftId" integer NOT NULL,
    "PhysicianId" integer NOT NULL,
    "StartDate" date NOT NULL,
    "IsRepeat" bit(1) NOT NULL,
    "WeekDays" character(7),
    "RepeatUpto" integer,
    "CreatedBy" character varying(128) NOT NULL,
    "CreatedDate" timestamp without time zone NOT NULL,
    "IP" character varying(20)
);
    DROP TABLE public."Shift";
       public         heap    postgres    false                       1259    33322    ShiftDetail    TABLE     "  CREATE TABLE public."ShiftDetail" (
    "ShiftDetailId" integer NOT NULL,
    "ShiftId" integer NOT NULL,
    "ShiftDate" timestamp without time zone NOT NULL,
    "RegionId" integer,
    "StartTime" time without time zone NOT NULL,
    "EndTime" time without time zone NOT NULL,
    "Status" smallint NOT NULL,
    "IsDeleted" bit(1) NOT NULL,
    "ModifiedBy" character varying(128),
    "ModifiedDate" timestamp without time zone,
    "LastRunningDate" timestamp without time zone,
    "EventId" character varying(100),
    "IsSync" bit(1)
);
 !   DROP TABLE public."ShiftDetail";
       public         heap    postgres    false                       1259    33339    ShiftDetailRegion    TABLE     �   CREATE TABLE public."ShiftDetailRegion" (
    "ShiftDetailRegionId" integer NOT NULL,
    "ShiftDetailId" integer NOT NULL,
    "RegionId" integer NOT NULL,
    "IsDeleted" bit(1)
);
 '   DROP TABLE public."ShiftDetailRegion";
       public         heap    postgres    false                       1259    33338 )   ShiftDetailRegion_ShiftDetailRegionId_seq    SEQUENCE     �   CREATE SEQUENCE public."ShiftDetailRegion_ShiftDetailRegionId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 B   DROP SEQUENCE public."ShiftDetailRegion_ShiftDetailRegionId_seq";
       public          postgres    false    277            �           0    0 )   ShiftDetailRegion_ShiftDetailRegionId_seq    SEQUENCE OWNED BY     }   ALTER SEQUENCE public."ShiftDetailRegion_ShiftDetailRegionId_seq" OWNED BY public."ShiftDetailRegion"."ShiftDetailRegionId";
          public          postgres    false    276                       1259    33321    ShiftDetail_ShiftDetailId_seq    SEQUENCE     �   CREATE SEQUENCE public."ShiftDetail_ShiftDetailId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 6   DROP SEQUENCE public."ShiftDetail_ShiftDetailId_seq";
       public          postgres    false    275            �           0    0    ShiftDetail_ShiftDetailId_seq    SEQUENCE OWNED BY     e   ALTER SEQUENCE public."ShiftDetail_ShiftDetailId_seq" OWNED BY public."ShiftDetail"."ShiftDetailId";
          public          postgres    false    274                       1259    33304    Shift_ShiftId_seq    SEQUENCE     �   CREATE SEQUENCE public."Shift_ShiftId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public."Shift_ShiftId_seq";
       public          postgres    false    273            �           0    0    Shift_ShiftId_seq    SEQUENCE OWNED BY     M   ALTER SEQUENCE public."Shift_ShiftId_seq" OWNED BY public."Shift"."ShiftId";
          public          postgres    false    272            �            1259    32982    User    TABLE     e  CREATE TABLE public."User" (
    "UserId" integer NOT NULL,
    "Id" character varying(128),
    "FirstName" character varying(100) NOT NULL,
    "LastName" character varying(100),
    "Email" character varying(50) NOT NULL,
    "Mobile" character varying(20),
    "IsMobile" bit(1),
    "Street" character varying(100),
    "City" character varying(100),
    "State" character varying(100),
    "RegionId" integer,
    "ZipCode" character varying(10),
    "strMonth" character varying(20),
    "intYear" integer,
    "intDate" integer,
    "CreatedBy" character varying(128) NOT NULL,
    "CreatedDate" timestamp without time zone NOT NULL,
    "ModifiedBy" character varying(128),
    "ModifiedDate" timestamp without time zone,
    "Status" smallint,
    "IsDeleted" bit(1),
    "IP" character varying(20),
    "IsRequestWithEmail" bit(1),
    "DateOfBirth" date
);
    DROP TABLE public."User";
       public         heap    postgres    false                       1259    33372    User_UserId_seq    SEQUENCE     �   ALTER TABLE public."User" ALTER COLUMN "UserId" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."User_UserId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    234            �           2604    32858    Admin AdminId    DEFAULT     t   ALTER TABLE ONLY public."Admin" ALTER COLUMN "AdminId" SET DEFAULT nextval('public."Admin_AdminId_seq"'::regclass);
 @   ALTER TABLE public."Admin" ALTER COLUMN "AdminId" DROP DEFAULT;
       public          postgres    false    217    218    218                       2604    32927    AdminRegion AdminRegionId    DEFAULT     �   ALTER TABLE ONLY public."AdminRegion" ALTER COLUMN "AdminRegionId" SET DEFAULT nextval('public."AdminRegion_AdminRegionId_seq"'::regclass);
 L   ALTER TABLE public."AdminRegion" ALTER COLUMN "AdminRegionId" DROP DEFAULT;
       public          postgres    false    229    228    229            �           2604    32897    BlockRequests BlockRequestId    DEFAULT     �   ALTER TABLE ONLY public."BlockRequests" ALTER COLUMN "BlockRequestId" SET DEFAULT nextval('public."BlockRequests_BlockRequestId_seq"'::regclass);
 O   ALTER TABLE public."BlockRequests" ALTER COLUMN "BlockRequestId" DROP DEFAULT;
       public          postgres    false    220    221    221                       2604    32944    Business BusinessId    DEFAULT     �   ALTER TABLE ONLY public."Business" ALTER COLUMN "BusinessId" SET DEFAULT nextval('public."Business_BusinessId_seq"'::regclass);
 F   ALTER TABLE public."Business" ALTER COLUMN "BusinessId" DROP DEFAULT;
       public          postgres    false    231    230    231            �           2604    32906    BusinessType BusinessTypeId    DEFAULT     �   ALTER TABLE ONLY public."BusinessType" ALTER COLUMN "BusinessTypeId" SET DEFAULT nextval('public."BusinessType_BusinessTypeId_seq"'::regclass);
 N   ALTER TABLE public."BusinessType" ALTER COLUMN "BusinessTypeId" DROP DEFAULT;
       public          postgres    false    223    222    223            �           2604    32913    CaseTag CaseTagId    DEFAULT     |   ALTER TABLE ONLY public."CaseTag" ALTER COLUMN "CaseTagId" SET DEFAULT nextval('public."CaseTag_CaseTagId_seq"'::regclass);
 D   ALTER TABLE public."CaseTag" ALTER COLUMN "CaseTagId" DROP DEFAULT;
       public          postgres    false    224    225    225                       2604    32974    Concierge ConciergeId    DEFAULT     �   ALTER TABLE ONLY public."Concierge" ALTER COLUMN "ConciergeId" SET DEFAULT nextval('public."Concierge_ConciergeId_seq"'::regclass);
 H   ALTER TABLE public."Concierge" ALTER COLUMN "ConciergeId" DROP DEFAULT;
       public          postgres    false    232    233    233                       2604    33015 +   HealthProfessionalType HealthProfessionalId    DEFAULT     �   ALTER TABLE ONLY public."HealthProfessionalType" ALTER COLUMN "HealthProfessionalId" SET DEFAULT nextval('public."HealthProfessionalType_HealthProfessionalId_seq"'::regclass);
 ^   ALTER TABLE public."HealthProfessionalType" ALTER COLUMN "HealthProfessionalId" DROP DEFAULT;
       public          postgres    false    236    237    237                       2604    33022    HealthProfessionals VendorId    DEFAULT     �   ALTER TABLE ONLY public."HealthProfessionals" ALTER COLUMN "VendorId" SET DEFAULT nextval('public."HealthProfessionals_VendorId_seq"'::regclass);
 O   ALTER TABLE public."HealthProfessionals" ALTER COLUMN "VendorId" DROP DEFAULT;
       public          postgres    false    239    238    239                       2604    33283    Menu MenuId    DEFAULT     p   ALTER TABLE ONLY public."Menu" ALTER COLUMN "MenuId" SET DEFAULT nextval('public."Menu_MenuId_seq"'::regclass);
 >   ALTER TABLE public."Menu" ALTER COLUMN "MenuId" DROP DEFAULT;
       public          postgres    false    268    269    269                       2604    33036    OrderDetails Id    DEFAULT     x   ALTER TABLE ONLY public."OrderDetails" ALTER COLUMN "Id" SET DEFAULT nextval('public."OrderDetails_Id_seq"'::regclass);
 B   ALTER TABLE public."OrderDetails" ALTER COLUMN "Id" DROP DEFAULT;
       public          postgres    false    240    241    241                       2604    33045    Physician PhysicianId    DEFAULT     �   ALTER TABLE ONLY public."Physician" ALTER COLUMN "PhysicianId" SET DEFAULT nextval('public."Physician_PhysicianId_seq"'::regclass);
 H   ALTER TABLE public."Physician" ALTER COLUMN "PhysicianId" DROP DEFAULT;
       public          postgres    false    243    242    243                       2604    33367    PhysicianLocation LocationId    DEFAULT     �   ALTER TABLE ONLY public."PhysicianLocation" ALTER COLUMN "LocationId" SET DEFAULT nextval('public."PhysicianLocation_LocationId_seq"'::regclass);
 O   ALTER TABLE public."PhysicianLocation" ALTER COLUMN "LocationId" DROP DEFAULT;
       public          postgres    false    280    279    280                       2604    33069    PhysicianNotification id    DEFAULT     �   ALTER TABLE ONLY public."PhysicianNotification" ALTER COLUMN id SET DEFAULT nextval('public."PhysicianNotification_id_seq"'::regclass);
 I   ALTER TABLE public."PhysicianNotification" ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    244    245    245            	           2604    33081 !   PhysicianRegion PhysicianRegionId    DEFAULT     �   ALTER TABLE ONLY public."PhysicianRegion" ALTER COLUMN "PhysicianRegionId" SET DEFAULT nextval('public."PhysicianRegion_PhysicianRegionId_seq"'::regclass);
 T   ALTER TABLE public."PhysicianRegion" ALTER COLUMN "PhysicianRegionId" DROP DEFAULT;
       public          postgres    false    246    247    247                        2604    32920    Region RegionId    DEFAULT     x   ALTER TABLE ONLY public."Region" ALTER COLUMN "RegionId" SET DEFAULT nextval('public."Region_RegionId_seq"'::regclass);
 B   ALTER TABLE public."Region" ALTER COLUMN "RegionId" DROP DEFAULT;
       public          postgres    false    227    226    227            
           2604    33119 !   RequestBusiness RequestBusinessId    DEFAULT     �   ALTER TABLE ONLY public."RequestBusiness" ALTER COLUMN "RequestBusinessId" SET DEFAULT nextval('public."RequestBusiness_RequestBusinessId_seq"'::regclass);
 T   ALTER TABLE public."RequestBusiness" ALTER COLUMN "RequestBusinessId" DROP DEFAULT;
       public          postgres    false    250    251    251                       2604    33136    RequestClient RequestClientId    DEFAULT     �   ALTER TABLE ONLY public."RequestClient" ALTER COLUMN "RequestClientId" SET DEFAULT nextval('public."RequestClient_RequestClientId_seq"'::regclass);
 P   ALTER TABLE public."RequestClient" ALTER COLUMN "RequestClientId" DROP DEFAULT;
       public          postgres    false    253    252    253                       2604    33184    RequestClosed RequestClosedId    DEFAULT     �   ALTER TABLE ONLY public."RequestClosed" ALTER COLUMN "RequestClosedId" SET DEFAULT nextval('public."RequestClosed_RequestClosedId_seq"'::regclass);
 P   ALTER TABLE public."RequestClosed" ALTER COLUMN "RequestClosedId" DROP DEFAULT;
       public          postgres    false    257    256    257                       2604    33203    RequestConcierge Id    DEFAULT     �   ALTER TABLE ONLY public."RequestConcierge" ALTER COLUMN "Id" SET DEFAULT nextval('public."RequestConcierge_Id_seq"'::regclass);
 F   ALTER TABLE public."RequestConcierge" ALTER COLUMN "Id" DROP DEFAULT;
       public          postgres    false    259    258    259                       2604    33155 #   RequestStatusLog RequestStatusLogId    DEFAULT     �   ALTER TABLE ONLY public."RequestStatusLog" ALTER COLUMN "RequestStatusLogId" SET DEFAULT nextval('public."RequestStatusLog_RequestStatusLogId_seq"'::regclass);
 V   ALTER TABLE public."RequestStatusLog" ALTER COLUMN "RequestStatusLogId" DROP DEFAULT;
       public          postgres    false    255    254    255                       2604    33244    RequestType RequestTypeId    DEFAULT     �   ALTER TABLE ONLY public."RequestType" ALTER COLUMN "RequestTypeId" SET DEFAULT nextval('public."RequestType_RequestTypeId_seq"'::regclass);
 L   ALTER TABLE public."RequestType" ALTER COLUMN "RequestTypeId" DROP DEFAULT;
       public          postgres    false    263    262    263                       2604    33251 !   RequestWiseFile RequestWiseFileID    DEFAULT     �   ALTER TABLE ONLY public."RequestWiseFile" ALTER COLUMN "RequestWiseFileID" SET DEFAULT nextval('public."RequestWiseFile_RequestWiseFileID_seq"'::regclass);
 T   ALTER TABLE public."RequestWiseFile" ALTER COLUMN "RequestWiseFileID" DROP DEFAULT;
       public          postgres    false    264    265    265                       2604    33275    Role RoleId    DEFAULT     p   ALTER TABLE ONLY public."Role" ALTER COLUMN "RoleId" SET DEFAULT nextval('public."Role_RoleId_seq"'::regclass);
 >   ALTER TABLE public."Role" ALTER COLUMN "RoleId" DROP DEFAULT;
       public          postgres    false    267    266    267                       2604    33291    RoleMenu RoleMenuId    DEFAULT     �   ALTER TABLE ONLY public."RoleMenu" ALTER COLUMN "RoleMenuId" SET DEFAULT nextval('public."RoleMenu_RoleMenuId_seq"'::regclass);
 F   ALTER TABLE public."RoleMenu" ALTER COLUMN "RoleMenuId" DROP DEFAULT;
       public          postgres    false    270    271    271                       2604    33308    Shift ShiftId    DEFAULT     t   ALTER TABLE ONLY public."Shift" ALTER COLUMN "ShiftId" SET DEFAULT nextval('public."Shift_ShiftId_seq"'::regclass);
 @   ALTER TABLE public."Shift" ALTER COLUMN "ShiftId" DROP DEFAULT;
       public          postgres    false    273    272    273                       2604    33325    ShiftDetail ShiftDetailId    DEFAULT     �   ALTER TABLE ONLY public."ShiftDetail" ALTER COLUMN "ShiftDetailId" SET DEFAULT nextval('public."ShiftDetail_ShiftDetailId_seq"'::regclass);
 L   ALTER TABLE public."ShiftDetail" ALTER COLUMN "ShiftDetailId" DROP DEFAULT;
       public          postgres    false    275    274    275                       2604    33342 %   ShiftDetailRegion ShiftDetailRegionId    DEFAULT     �   ALTER TABLE ONLY public."ShiftDetailRegion" ALTER COLUMN "ShiftDetailRegionId" SET DEFAULT nextval('public."ShiftDetailRegion_ShiftDetailRegionId_seq"'::regclass);
 X   ALTER TABLE public."ShiftDetailRegion" ALTER COLUMN "ShiftDetailRegionId" DROP DEFAULT;
       public          postgres    false    277    276    277            &          0    32855    Admin 
   TABLE DATA             COPY public."Admin" ("AdminId", "AspNetUserId", "FirstName", "LastName", "Email", "Mobile", "Address1", "Address2", "City", "RegionId", "Zip", "AltPhone", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "Status", "IsDeleted", "RoleId") FROM stdin;
    public          postgres    false    218   A�      1          0    32924    AdminRegion 
   TABLE DATA           O   COPY public."AdminRegion" ("AdminRegionId", "AdminId", "RegionId") FROM stdin;
    public          postgres    false    229   ��      $          0    32849    AspNetRoles 
   TABLE DATA           5   COPY public."AspNetRoles" ("Id", "Name") FROM stdin;
    public          postgres    false    216   Ě      '          0    32878    AspNetUserRoles 
   TABLE DATA           ?   COPY public."AspNetUserRoles" ("UserId", "RoleId") FROM stdin;
    public          postgres    false    219   �      #          0    32842    AspNetUsers 
   TABLE DATA           �   COPY public."AspNetUsers" ("Id", "UserName", "PasswordHash", "Email", "PhoneNumber", "IP", "CreatedDate", "ModifiedDate") FROM stdin;
    public          postgres    false    215   ��      )          0    32894    BlockRequests 
   TABLE DATA           �   COPY public."BlockRequests" ("BlockRequestId", "PhoneNumber", "Email", "IsActive", "Reason", "RequestId", "IP", "CreatedDate", "ModifiedDate") FROM stdin;
    public          postgres    false    221   ��      3          0    32941    Business 
   TABLE DATA             COPY public."Business" ("BusinessId", "Name", "BusinessTypeId", "Address1", "Address2", "City", "RegionId", "ZipCode", "PhoneNumber", "FaxNumber", "IsRegistered", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "Status", "IsDeleted", "IP") FROM stdin;
    public          postgres    false    231   ��      +          0    32903    BusinessType 
   TABLE DATA           B   COPY public."BusinessType" ("BusinessTypeId", "Name") FROM stdin;
    public          postgres    false    223   �      -          0    32910    CaseTag 
   TABLE DATA           8   COPY public."CaseTag" ("CaseTagId", "Name") FROM stdin;
    public          postgres    false    225   %�      5          0    32971 	   Concierge 
   TABLE DATA           �   COPY public."Concierge" ("ConciergeId", "ConciergeName", "Address", "Street", "City", "State", "ZipCode", "CreatedDate", "RegionId", "IP") FROM stdin;
    public          postgres    false    233   B�      7          0    33004    EmailLog 
   TABLE DATA           �   COPY public."EmailLog" ("EmailLogID", "EmailTemplate", "SubjectName", "EmailID", "ConfirmationNumber", "FilePath", "RoleId", "RequestId", "AdminId", "PhysicianId", "CreateDate", "SentDate", "IsEmailSent", "SentTries", "Action") FROM stdin;
    public          postgres    false    235   O�      9          0    33012    HealthProfessionalType 
   TABLE DATA           �   COPY public."HealthProfessionalType" ("HealthProfessionalId", "ProfessionName", "CreatedDate", "IsActive", "IsDeleted") FROM stdin;
    public          postgres    false    237   l�      ;          0    33019    HealthProfessionals 
   TABLE DATA           �   COPY public."HealthProfessionals" ("VendorId", "VendorName", "Profession", "FaxNumber", "Address", "City", "State", "Zip", "RegionId", "CreatedDate", "ModifiedDate", "PhoneNumber", "IsDeleted", "IP", "Email", "BusinessContact") FROM stdin;
    public          postgres    false    239   ��      Y          0    33280    Menu 
   TABLE DATA           N   COPY public."Menu" ("MenuId", "Name", "AccountType", "SortOrder") FROM stdin;
    public          postgres    false    269   ��      =          0    33033    OrderDetails 
   TABLE DATA           �   COPY public."OrderDetails" ("Id", "VendorId", "RequestId", "FaxNumber", "Email", "BusinessContact", "Prescription", "NoOfRefill", "CreatedDate", "CreatedBy") FROM stdin;
    public          postgres    false    241   à      ?          0    33042 	   Physician 
   TABLE DATA           �  COPY public."Physician" ("PhysicianId", "Id", "FirstName", "LastName", "Email", "Mobile", "MedicalLicense", "Photo", "AdminNotes", "IsAgreementDoc", "IsBackgroundDoc", "IsTrainingDoc", "IsNonDisclosureDoc", "Address1", "Address2", "City", "RegionId", "Zip", "AltPhone", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "Status", "BusinessName", "BusinessWebsite", "IsDeleted", "RoleId", "NPINumber", "IsLicenseDoc", "Signature", "IsCredentialDoc", "IsTokenGenerate", "SyncEmailAddress") FROM stdin;
    public          postgres    false    243   �      d          0    33364    PhysicianLocation 
   TABLE DATA           �   COPY public."PhysicianLocation" ("LocationId", "PhysicianId", "Latitude", "Longitude", "CreatedDate", "PhysicianName", "Address") FROM stdin;
    public          postgres    false    280   g�      A          0    33066    PhysicianNotification 
   TABLE DATA           ]   COPY public."PhysicianNotification" (id, "PhysicianId", "IsNotificationStopped") FROM stdin;
    public          postgres    false    245   ��      C          0    33078    PhysicianRegion 
   TABLE DATA           [   COPY public."PhysicianRegion" ("PhysicianRegionId", "PhysicianId", "RegionId") FROM stdin;
    public          postgres    false    247   ��      /          0    32917    Region 
   TABLE DATA           F   COPY public."Region" ("RegionId", "Name", "Abbreviation") FROM stdin;
    public          postgres    false    227   ��      E          0    33095    Request 
   TABLE DATA           �  COPY public."Request" ("RequestId", "RequestTypeId", "UserId", "FirstName", "LastName", "PhoneNumber", "Email", "Status", "PhysicianId", "ConfirmationNumber", "CreatedDate", "IsDeleted", "ModifiedDate", "DeclinedBy", "IsUrgentEmailSent", "LastWellnessDate", "IsMobile", "CallType", "CompletedByPhysician", "LastReservationDate", "AcceptedDate", "RelationName", "CaseNumber", "IP", "CaseTag", "CaseTagPhysician", "PatientAccountId", "CreatedUserId") FROM stdin;
    public          postgres    false    249   �      G          0    33116    RequestBusiness 
   TABLE DATA           a   COPY public."RequestBusiness" ("RequestBusinessId", "RequestId", "BusinessId", "IP") FROM stdin;
    public          postgres    false    251   (�      I          0    33133    RequestClient 
   TABLE DATA           �  COPY public."RequestClient" ("RequestClientId", "RequestId", "FirstName", "LastName", "PhoneNumber", "Location", "Address", "RegionId", "NotiMobile", "NotiEmail", "Notes", "Email", "strMonth", "intYear", "intDate", "IsMobile", "Street", "City", "State", "ZipCode", "CommunicationType", "RemindReservationCount", "RemindHouseCallCount", "IsSetFollowupSent", "IP", "IsReservationReminderSent", "Latitude", "Longitude", "DateOfBirth") FROM stdin;
    public          postgres    false    253   E�      M          0    33181    RequestClosed 
   TABLE DATA           �   COPY public."RequestClosed" ("RequestClosedId", "RequestId", "RequestStatusLogId", "PhyNotes", "ClientNotes", "IP") FROM stdin;
    public          postgres    false    257   ��      O          0    33200    RequestConcierge 
   TABLE DATA           T   COPY public."RequestConcierge" ("Id", "RequestId", "ConciergeId", "IP") FROM stdin;
    public          postgres    false    259   ˦      Q          0    33217    RequestNotes 
   TABLE DATA           �   COPY public."RequestNotes" ("RequestNotesId", "RequestId", "strMonth", "intYear", "intDate", "PhysicianNotes", "AdminNotes", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "IP", "AdministrativeNotes") FROM stdin;
    public          postgres    false    261   �      K          0    33152    RequestStatusLog 
   TABLE DATA           �   COPY public."RequestStatusLog" ("RequestStatusLogId", "RequestId", "Status", "PhysicianId", "AdminId", "TransToPhysicianId", "Notes", "CreatedDate", "IP", "TransToAdmin") FROM stdin;
    public          postgres    false    255   ȧ      S          0    33241    RequestType 
   TABLE DATA           @   COPY public."RequestType" ("RequestTypeId", "Name") FROM stdin;
    public          postgres    false    263   �      U          0    33248    RequestWiseFile 
   TABLE DATA           �   COPY public."RequestWiseFile" ("RequestWiseFileID", "RequestId", "FileName", "CreatedDate", "PhysicianId", "AdminId", "DocType", "IsFrontSide", "IsCompensation", "IP", "IsFinalize", "IsDeleted", "IsPatientRecords") FROM stdin;
    public          postgres    false    265   \�      W          0    33272    Role 
   TABLE DATA           �   COPY public."Role" ("RoleId", "Name", "AccountType", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "IsDeleted", "IP") FROM stdin;
    public          postgres    false    267   �      [          0    33288    RoleMenu 
   TABLE DATA           F   COPY public."RoleMenu" ("RoleMenuId", "RoleId", "MenuId") FROM stdin;
    public          postgres    false    271   2�      b          0    33356    SMSLog 
   TABLE DATA           �   COPY public."SMSLog" ("SMSLogID", "SMSTemplate", "MobileNumber", "ConfirmationNumber", "RoleId", "AdminId", "RequestId", "PhysicianId", "CreateDate", "SentDate", "IsSMSSent", "SentTries", "Action") FROM stdin;
    public          postgres    false    278   O�      ]          0    33305    Shift 
   TABLE DATA           �   COPY public."Shift" ("ShiftId", "PhysicianId", "StartDate", "IsRepeat", "WeekDays", "RepeatUpto", "CreatedBy", "CreatedDate", "IP") FROM stdin;
    public          postgres    false    273   l�      _          0    33322    ShiftDetail 
   TABLE DATA           �   COPY public."ShiftDetail" ("ShiftDetailId", "ShiftId", "ShiftDate", "RegionId", "StartTime", "EndTime", "Status", "IsDeleted", "ModifiedBy", "ModifiedDate", "LastRunningDate", "EventId", "IsSync") FROM stdin;
    public          postgres    false    275   ��      a          0    33339    ShiftDetailRegion 
   TABLE DATA           n   COPY public."ShiftDetailRegion" ("ShiftDetailRegionId", "ShiftDetailId", "RegionId", "IsDeleted") FROM stdin;
    public          postgres    false    277   ��      6          0    32982    User 
   TABLE DATA           8  COPY public."User" ("UserId", "Id", "FirstName", "LastName", "Email", "Mobile", "IsMobile", "Street", "City", "State", "RegionId", "ZipCode", "strMonth", "intYear", "intDate", "CreatedBy", "CreatedDate", "ModifiedBy", "ModifiedDate", "Status", "IsDeleted", "IP", "IsRequestWithEmail", "DateOfBirth") FROM stdin;
    public          postgres    false    234   ê      �           0    0    AdminRegion_AdminRegionId_seq    SEQUENCE SET     N   SELECT pg_catalog.setval('public."AdminRegion_AdminRegionId_seq"', 1, false);
          public          postgres    false    228            �           0    0    Admin_AdminId_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public."Admin_AdminId_seq"', 1, false);
          public          postgres    false    217            �           0    0     BlockRequests_BlockRequestId_seq    SEQUENCE SET     Q   SELECT pg_catalog.setval('public."BlockRequests_BlockRequestId_seq"', 1, false);
          public          postgres    false    220            �           0    0    BusinessType_BusinessTypeId_seq    SEQUENCE SET     P   SELECT pg_catalog.setval('public."BusinessType_BusinessTypeId_seq"', 1, false);
          public          postgres    false    222            �           0    0    Business_BusinessId_seq    SEQUENCE SET     H   SELECT pg_catalog.setval('public."Business_BusinessId_seq"', 19, true);
          public          postgres    false    230            �           0    0    CaseTag_CaseTagId_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public."CaseTag_CaseTagId_seq"', 1, false);
          public          postgres    false    224            �           0    0    Concierge_ConciergeId_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public."Concierge_ConciergeId_seq"', 20, true);
          public          postgres    false    232            �           0    0 /   HealthProfessionalType_HealthProfessionalId_seq    SEQUENCE SET     `   SELECT pg_catalog.setval('public."HealthProfessionalType_HealthProfessionalId_seq"', 1, false);
          public          postgres    false    236            �           0    0     HealthProfessionals_VendorId_seq    SEQUENCE SET     Q   SELECT pg_catalog.setval('public."HealthProfessionals_VendorId_seq"', 1, false);
          public          postgres    false    238            �           0    0    Menu_MenuId_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public."Menu_MenuId_seq"', 1, false);
          public          postgres    false    268            �           0    0    OrderDetails_Id_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public."OrderDetails_Id_seq"', 1, false);
          public          postgres    false    240            �           0    0     PhysicianLocation_LocationId_seq    SEQUENCE SET     Q   SELECT pg_catalog.setval('public."PhysicianLocation_LocationId_seq"', 1, false);
          public          postgres    false    279            �           0    0    PhysicianNotification_id_seq    SEQUENCE SET     M   SELECT pg_catalog.setval('public."PhysicianNotification_id_seq"', 1, false);
          public          postgres    false    244            �           0    0 %   PhysicianRegion_PhysicianRegionId_seq    SEQUENCE SET     V   SELECT pg_catalog.setval('public."PhysicianRegion_PhysicianRegionId_seq"', 1, false);
          public          postgres    false    246            �           0    0    Physician_PhysicianId_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public."Physician_PhysicianId_seq"', 1, false);
          public          postgres    false    242            �           0    0    Region_RegionId_seq    SEQUENCE SET     D   SELECT pg_catalog.setval('public."Region_RegionId_seq"', 1, false);
          public          postgres    false    226            �           0    0 %   RequestBusiness_RequestBusinessId_seq    SEQUENCE SET     V   SELECT pg_catalog.setval('public."RequestBusiness_RequestBusinessId_seq"', 1, false);
          public          postgres    false    250            �           0    0 !   RequestClient_RequestClientId_seq    SEQUENCE SET     S   SELECT pg_catalog.setval('public."RequestClient_RequestClientId_seq"', 235, true);
          public          postgres    false    252            �           0    0 !   RequestClosed_RequestClosedId_seq    SEQUENCE SET     R   SELECT pg_catalog.setval('public."RequestClosed_RequestClosedId_seq"', 1, false);
          public          postgres    false    256            �           0    0    RequestConcierge_Id_seq    SEQUENCE SET     H   SELECT pg_catalog.setval('public."RequestConcierge_Id_seq"', 1, false);
          public          postgres    false    258            �           0    0    RequestNotes_RequestNotesId_seq    SEQUENCE SET     O   SELECT pg_catalog.setval('public."RequestNotes_RequestNotesId_seq"', 1, true);
          public          postgres    false    260            �           0    0     RequestNotes_RequestNotesId_seq1    SEQUENCE SET     Q   SELECT pg_catalog.setval('public."RequestNotes_RequestNotesId_seq1"', 13, true);
          public          postgres    false    283            �           0    0 '   RequestStatusLog_RequestStatusLogId_seq    SEQUENCE SET     X   SELECT pg_catalog.setval('public."RequestStatusLog_RequestStatusLogId_seq"', 1, false);
          public          postgres    false    254            �           0    0    RequestType_RequestTypeId_seq    SEQUENCE SET     M   SELECT pg_catalog.setval('public."RequestType_RequestTypeId_seq"', 1, true);
          public          postgres    false    262            �           0    0 %   RequestWiseFile_RequestWiseFileID_seq    SEQUENCE SET     W   SELECT pg_catalog.setval('public."RequestWiseFile_RequestWiseFileID_seq"', 208, true);
          public          postgres    false    264            �           0    0    Request_RequestId_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public."Request_RequestId_seq"', 45, true);
          public          postgres    false    248            �           0    0    Request_RequestId_seq1    SEQUENCE SET     H   SELECT pg_catalog.setval('public."Request_RequestId_seq1"', 250, true);
          public          postgres    false    282            �           0    0    RoleMenu_RoleMenuId_seq    SEQUENCE SET     H   SELECT pg_catalog.setval('public."RoleMenu_RoleMenuId_seq"', 1, false);
          public          postgres    false    270            �           0    0    Role_RoleId_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public."Role_RoleId_seq"', 1, false);
          public          postgres    false    266            �           0    0 )   ShiftDetailRegion_ShiftDetailRegionId_seq    SEQUENCE SET     Z   SELECT pg_catalog.setval('public."ShiftDetailRegion_ShiftDetailRegionId_seq"', 1, false);
          public          postgres    false    276            �           0    0    ShiftDetail_ShiftDetailId_seq    SEQUENCE SET     N   SELECT pg_catalog.setval('public."ShiftDetail_ShiftDetailId_seq"', 1, false);
          public          postgres    false    274            �           0    0    Shift_ShiftId_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public."Shift_ShiftId_seq"', 1, false);
          public          postgres    false    272            �           0    0    User_UserId_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public."User_UserId_seq"', 176, true);
          public          postgres    false    281            -           2606    32929    AdminRegion AdminRegion_pkey 
   CONSTRAINT     k   ALTER TABLE ONLY public."AdminRegion"
    ADD CONSTRAINT "AdminRegion_pkey" PRIMARY KEY ("AdminRegionId");
 J   ALTER TABLE ONLY public."AdminRegion" DROP CONSTRAINT "AdminRegion_pkey";
       public            postgres    false    229            !           2606    32862    Admin Admin_pkey 
   CONSTRAINT     Y   ALTER TABLE ONLY public."Admin"
    ADD CONSTRAINT "Admin_pkey" PRIMARY KEY ("AdminId");
 >   ALTER TABLE ONLY public."Admin" DROP CONSTRAINT "Admin_pkey";
       public            postgres    false    218                       2606    32853    AspNetRoles AspNetRoles_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public."AspNetRoles"
    ADD CONSTRAINT "AspNetRoles_pkey" PRIMARY KEY ("Id");
 J   ALTER TABLE ONLY public."AspNetRoles" DROP CONSTRAINT "AspNetRoles_pkey";
       public            postgres    false    216            #           2606    32882 $   AspNetUserRoles AspNetUserRoles_pkey 
   CONSTRAINT     v   ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "AspNetUserRoles_pkey" PRIMARY KEY ("UserId", "RoleId");
 R   ALTER TABLE ONLY public."AspNetUserRoles" DROP CONSTRAINT "AspNetUserRoles_pkey";
       public            postgres    false    219    219                       2606    32848    AspNetUsers AspNetUsers_pkey 
   CONSTRAINT     `   ALTER TABLE ONLY public."AspNetUsers"
    ADD CONSTRAINT "AspNetUsers_pkey" PRIMARY KEY ("Id");
 J   ALTER TABLE ONLY public."AspNetUsers" DROP CONSTRAINT "AspNetUsers_pkey";
       public            postgres    false    215            %           2606    32901     BlockRequests BlockRequests_pkey 
   CONSTRAINT     p   ALTER TABLE ONLY public."BlockRequests"
    ADD CONSTRAINT "BlockRequests_pkey" PRIMARY KEY ("BlockRequestId");
 N   ALTER TABLE ONLY public."BlockRequests" DROP CONSTRAINT "BlockRequests_pkey";
       public            postgres    false    221            '           2606    32908    BusinessType BusinessType_pkey 
   CONSTRAINT     n   ALTER TABLE ONLY public."BusinessType"
    ADD CONSTRAINT "BusinessType_pkey" PRIMARY KEY ("BusinessTypeId");
 L   ALTER TABLE ONLY public."BusinessType" DROP CONSTRAINT "BusinessType_pkey";
       public            postgres    false    223            /           2606    32948    Business Business_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public."Business"
    ADD CONSTRAINT "Business_pkey" PRIMARY KEY ("BusinessId");
 D   ALTER TABLE ONLY public."Business" DROP CONSTRAINT "Business_pkey";
       public            postgres    false    231            )           2606    32915    CaseTag CaseTag_pkey 
   CONSTRAINT     _   ALTER TABLE ONLY public."CaseTag"
    ADD CONSTRAINT "CaseTag_pkey" PRIMARY KEY ("CaseTagId");
 B   ALTER TABLE ONLY public."CaseTag" DROP CONSTRAINT "CaseTag_pkey";
       public            postgres    false    225            1           2606    32976    Concierge Concierge_pkey 
   CONSTRAINT     e   ALTER TABLE ONLY public."Concierge"
    ADD CONSTRAINT "Concierge_pkey" PRIMARY KEY ("ConciergeId");
 F   ALTER TABLE ONLY public."Concierge" DROP CONSTRAINT "Concierge_pkey";
       public            postgres    false    233            5           2606    33010    EmailLog EmailLog_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public."EmailLog"
    ADD CONSTRAINT "EmailLog_pkey" PRIMARY KEY ("EmailLogID");
 D   ALTER TABLE ONLY public."EmailLog" DROP CONSTRAINT "EmailLog_pkey";
       public            postgres    false    235            7           2606    33017 2   HealthProfessionalType HealthProfessionalType_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public."HealthProfessionalType"
    ADD CONSTRAINT "HealthProfessionalType_pkey" PRIMARY KEY ("HealthProfessionalId");
 `   ALTER TABLE ONLY public."HealthProfessionalType" DROP CONSTRAINT "HealthProfessionalType_pkey";
       public            postgres    false    237            9           2606    33026 ,   HealthProfessionals HealthProfessionals_pkey 
   CONSTRAINT     v   ALTER TABLE ONLY public."HealthProfessionals"
    ADD CONSTRAINT "HealthProfessionals_pkey" PRIMARY KEY ("VendorId");
 Z   ALTER TABLE ONLY public."HealthProfessionals" DROP CONSTRAINT "HealthProfessionals_pkey";
       public            postgres    false    239            W           2606    33286    Menu Menu_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public."Menu"
    ADD CONSTRAINT "Menu_pkey" PRIMARY KEY ("MenuId");
 <   ALTER TABLE ONLY public."Menu" DROP CONSTRAINT "Menu_pkey";
       public            postgres    false    269            ;           2606    33040    OrderDetails OrderDetails_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public."OrderDetails"
    ADD CONSTRAINT "OrderDetails_pkey" PRIMARY KEY ("Id");
 L   ALTER TABLE ONLY public."OrderDetails" DROP CONSTRAINT "OrderDetails_pkey";
       public            postgres    false    241            c           2606    33371 (   PhysicianLocation PhysicianLocation_pkey 
   CONSTRAINT     t   ALTER TABLE ONLY public."PhysicianLocation"
    ADD CONSTRAINT "PhysicianLocation_pkey" PRIMARY KEY ("LocationId");
 V   ALTER TABLE ONLY public."PhysicianLocation" DROP CONSTRAINT "PhysicianLocation_pkey";
       public            postgres    false    280            ?           2606    33071 0   PhysicianNotification PhysicianNotification_pkey 
   CONSTRAINT     r   ALTER TABLE ONLY public."PhysicianNotification"
    ADD CONSTRAINT "PhysicianNotification_pkey" PRIMARY KEY (id);
 ^   ALTER TABLE ONLY public."PhysicianNotification" DROP CONSTRAINT "PhysicianNotification_pkey";
       public            postgres    false    245            A           2606    33083 $   PhysicianRegion PhysicianRegion_pkey 
   CONSTRAINT     w   ALTER TABLE ONLY public."PhysicianRegion"
    ADD CONSTRAINT "PhysicianRegion_pkey" PRIMARY KEY ("PhysicianRegionId");
 R   ALTER TABLE ONLY public."PhysicianRegion" DROP CONSTRAINT "PhysicianRegion_pkey";
       public            postgres    false    247            =           2606    33049    Physician Physician_pkey 
   CONSTRAINT     e   ALTER TABLE ONLY public."Physician"
    ADD CONSTRAINT "Physician_pkey" PRIMARY KEY ("PhysicianId");
 F   ALTER TABLE ONLY public."Physician" DROP CONSTRAINT "Physician_pkey";
       public            postgres    false    243            +           2606    32922    Region Region_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public."Region"
    ADD CONSTRAINT "Region_pkey" PRIMARY KEY ("RegionId");
 @   ALTER TABLE ONLY public."Region" DROP CONSTRAINT "Region_pkey";
       public            postgres    false    227            E           2606    33121 $   RequestBusiness RequestBusiness_pkey 
   CONSTRAINT     w   ALTER TABLE ONLY public."RequestBusiness"
    ADD CONSTRAINT "RequestBusiness_pkey" PRIMARY KEY ("RequestBusinessId");
 R   ALTER TABLE ONLY public."RequestBusiness" DROP CONSTRAINT "RequestBusiness_pkey";
       public            postgres    false    251            G           2606    33140     RequestClient RequestClient_pkey 
   CONSTRAINT     q   ALTER TABLE ONLY public."RequestClient"
    ADD CONSTRAINT "RequestClient_pkey" PRIMARY KEY ("RequestClientId");
 N   ALTER TABLE ONLY public."RequestClient" DROP CONSTRAINT "RequestClient_pkey";
       public            postgres    false    253            K           2606    33188     RequestClosed RequestClosed_pkey 
   CONSTRAINT     q   ALTER TABLE ONLY public."RequestClosed"
    ADD CONSTRAINT "RequestClosed_pkey" PRIMARY KEY ("RequestClosedId");
 N   ALTER TABLE ONLY public."RequestClosed" DROP CONSTRAINT "RequestClosed_pkey";
       public            postgres    false    257            M           2606    33205 &   RequestConcierge RequestConcierge_pkey 
   CONSTRAINT     j   ALTER TABLE ONLY public."RequestConcierge"
    ADD CONSTRAINT "RequestConcierge_pkey" PRIMARY KEY ("Id");
 T   ALTER TABLE ONLY public."RequestConcierge" DROP CONSTRAINT "RequestConcierge_pkey";
       public            postgres    false    259            O           2606    33224    RequestNotes RequestNotes_pkey 
   CONSTRAINT     n   ALTER TABLE ONLY public."RequestNotes"
    ADD CONSTRAINT "RequestNotes_pkey" PRIMARY KEY ("RequestNotesId");
 L   ALTER TABLE ONLY public."RequestNotes" DROP CONSTRAINT "RequestNotes_pkey";
       public            postgres    false    261            I           2606    33159 &   RequestStatusLog RequestStatusLog_pkey 
   CONSTRAINT     z   ALTER TABLE ONLY public."RequestStatusLog"
    ADD CONSTRAINT "RequestStatusLog_pkey" PRIMARY KEY ("RequestStatusLogId");
 T   ALTER TABLE ONLY public."RequestStatusLog" DROP CONSTRAINT "RequestStatusLog_pkey";
       public            postgres    false    255            Q           2606    33246    RequestType RequestType_pkey 
   CONSTRAINT     k   ALTER TABLE ONLY public."RequestType"
    ADD CONSTRAINT "RequestType_pkey" PRIMARY KEY ("RequestTypeId");
 J   ALTER TABLE ONLY public."RequestType" DROP CONSTRAINT "RequestType_pkey";
       public            postgres    false    263            S           2606    33255 $   RequestWiseFile RequestWiseFile_pkey 
   CONSTRAINT     w   ALTER TABLE ONLY public."RequestWiseFile"
    ADD CONSTRAINT "RequestWiseFile_pkey" PRIMARY KEY ("RequestWiseFileID");
 R   ALTER TABLE ONLY public."RequestWiseFile" DROP CONSTRAINT "RequestWiseFile_pkey";
       public            postgres    false    265            C           2606    33104    Request Request_pkey 
   CONSTRAINT     _   ALTER TABLE ONLY public."Request"
    ADD CONSTRAINT "Request_pkey" PRIMARY KEY ("RequestId");
 B   ALTER TABLE ONLY public."Request" DROP CONSTRAINT "Request_pkey";
       public            postgres    false    249            Y           2606    33293    RoleMenu RoleMenu_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public."RoleMenu"
    ADD CONSTRAINT "RoleMenu_pkey" PRIMARY KEY ("RoleMenuId");
 D   ALTER TABLE ONLY public."RoleMenu" DROP CONSTRAINT "RoleMenu_pkey";
       public            postgres    false    271            U           2606    33278    Role Role_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public."Role"
    ADD CONSTRAINT "Role_pkey" PRIMARY KEY ("RoleId");
 <   ALTER TABLE ONLY public."Role" DROP CONSTRAINT "Role_pkey";
       public            postgres    false    267            a           2606    33362    SMSLog SMSLog_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public."SMSLog"
    ADD CONSTRAINT "SMSLog_pkey" PRIMARY KEY ("SMSLogID");
 @   ALTER TABLE ONLY public."SMSLog" DROP CONSTRAINT "SMSLog_pkey";
       public            postgres    false    278            _           2606    33344 (   ShiftDetailRegion ShiftDetailRegion_pkey 
   CONSTRAINT     }   ALTER TABLE ONLY public."ShiftDetailRegion"
    ADD CONSTRAINT "ShiftDetailRegion_pkey" PRIMARY KEY ("ShiftDetailRegionId");
 V   ALTER TABLE ONLY public."ShiftDetailRegion" DROP CONSTRAINT "ShiftDetailRegion_pkey";
       public            postgres    false    277            ]           2606    33327    ShiftDetail ShiftDetail_pkey 
   CONSTRAINT     k   ALTER TABLE ONLY public."ShiftDetail"
    ADD CONSTRAINT "ShiftDetail_pkey" PRIMARY KEY ("ShiftDetailId");
 J   ALTER TABLE ONLY public."ShiftDetail" DROP CONSTRAINT "ShiftDetail_pkey";
       public            postgres    false    275            [           2606    33310    Shift Shift_pkey 
   CONSTRAINT     Y   ALTER TABLE ONLY public."Shift"
    ADD CONSTRAINT "Shift_pkey" PRIMARY KEY ("ShiftId");
 >   ALTER TABLE ONLY public."Shift" DROP CONSTRAINT "Shift_pkey";
       public            postgres    false    273            3           2606    32988    User User_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("UserId");
 <   ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_pkey";
       public            postgres    false    234            d           2606    32863    Admin Admin_AspNetUserId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Admin"
    ADD CONSTRAINT "Admin_AspNetUserId_fkey" FOREIGN KEY ("AspNetUserId") REFERENCES public."AspNetUsers"("Id");
 K   ALTER TABLE ONLY public."Admin" DROP CONSTRAINT "Admin_AspNetUserId_fkey";
       public          postgres    false    215    218    4893            e           2606    32868    Admin Admin_CreatedBy_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Admin"
    ADD CONSTRAINT "Admin_CreatedBy_fkey" FOREIGN KEY ("CreatedBy") REFERENCES public."AspNetUsers"("Id");
 H   ALTER TABLE ONLY public."Admin" DROP CONSTRAINT "Admin_CreatedBy_fkey";
       public          postgres    false    218    4893    215            f           2606    32873    Admin Admin_ModifiedBy_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Admin"
    ADD CONSTRAINT "Admin_ModifiedBy_fkey" FOREIGN KEY ("ModifiedBy") REFERENCES public."AspNetUsers"("Id");
 I   ALTER TABLE ONLY public."Admin" DROP CONSTRAINT "Admin_ModifiedBy_fkey";
       public          postgres    false    215    4893    218            g           2606    32888 +   AspNetUserRoles AspNetUserRoles_RoleId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "AspNetUserRoles_RoleId_fkey" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id");
 Y   ALTER TABLE ONLY public."AspNetUserRoles" DROP CONSTRAINT "AspNetUserRoles_RoleId_fkey";
       public          postgres    false    216    219    4895            h           2606    32883 +   AspNetUserRoles AspNetUserRoles_UserId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "AspNetUserRoles_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id");
 Y   ALTER TABLE ONLY public."AspNetUserRoles" DROP CONSTRAINT "AspNetUserRoles_UserId_fkey";
       public          postgres    false    219    215    4893            k           2606    32949 %   Business Business_BusinessTypeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Business"
    ADD CONSTRAINT "Business_BusinessTypeId_fkey" FOREIGN KEY ("BusinessTypeId") REFERENCES public."BusinessType"("BusinessTypeId");
 S   ALTER TABLE ONLY public."Business" DROP CONSTRAINT "Business_BusinessTypeId_fkey";
       public          postgres    false    231    223    4903            l           2606    32964 !   Business Business_ModifiedBy_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Business"
    ADD CONSTRAINT "Business_ModifiedBy_fkey" FOREIGN KEY ("ModifiedBy") REFERENCES public."AspNetUsers"("Id");
 O   ALTER TABLE ONLY public."Business" DROP CONSTRAINT "Business_ModifiedBy_fkey";
       public          postgres    false    231    215    4893            m           2606    32954    Business Business_RegionId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Business"
    ADD CONSTRAINT "Business_RegionId_fkey" FOREIGN KEY ("RegionId") REFERENCES public."Region"("RegionId");
 M   ALTER TABLE ONLY public."Business" DROP CONSTRAINT "Business_RegionId_fkey";
       public          postgres    false    231    227    4907            n           2606    32977 !   Concierge Concierge_RegionId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Concierge"
    ADD CONSTRAINT "Concierge_RegionId_fkey" FOREIGN KEY ("RegionId") REFERENCES public."Region"("RegionId");
 O   ALTER TABLE ONLY public."Concierge" DROP CONSTRAINT "Concierge_RegionId_fkey";
       public          postgres    false    227    4907    233            i           2606    32930 "   AdminRegion FK_AdminRegion_AdminId    FK CONSTRAINT     �   ALTER TABLE ONLY public."AdminRegion"
    ADD CONSTRAINT "FK_AdminRegion_AdminId" FOREIGN KEY ("AdminId") REFERENCES public."Admin"("AdminId");
 P   ALTER TABLE ONLY public."AdminRegion" DROP CONSTRAINT "FK_AdminRegion_AdminId";
       public          postgres    false    218    4897    229            j           2606    32935 #   AdminRegion FK_AdminRegion_RegionId    FK CONSTRAINT     �   ALTER TABLE ONLY public."AdminRegion"
    ADD CONSTRAINT "FK_AdminRegion_RegionId" FOREIGN KEY ("RegionId") REFERENCES public."Region"("RegionId");
 Q   ALTER TABLE ONLY public."AdminRegion" DROP CONSTRAINT "FK_AdminRegion_RegionId";
       public          postgres    false    227    4907    229            q           2606    33027 7   HealthProfessionals HealthProfessionals_Profession_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."HealthProfessionals"
    ADD CONSTRAINT "HealthProfessionals_Profession_fkey" FOREIGN KEY ("Profession") REFERENCES public."HealthProfessionalType"("HealthProfessionalId");
 e   ALTER TABLE ONLY public."HealthProfessionals" DROP CONSTRAINT "HealthProfessionals_Profession_fkey";
       public          postgres    false    237    239    4919            u           2606    33072 <   PhysicianNotification PhysicianNotification_PhysicianId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."PhysicianNotification"
    ADD CONSTRAINT "PhysicianNotification_PhysicianId_fkey" FOREIGN KEY ("PhysicianId") REFERENCES public."Physician"("PhysicianId");
 j   ALTER TABLE ONLY public."PhysicianNotification" DROP CONSTRAINT "PhysicianNotification_PhysicianId_fkey";
       public          postgres    false    243    245    4925            v           2606    33084 0   PhysicianRegion PhysicianRegion_PhysicianId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."PhysicianRegion"
    ADD CONSTRAINT "PhysicianRegion_PhysicianId_fkey" FOREIGN KEY ("PhysicianId") REFERENCES public."Physician"("PhysicianId");
 ^   ALTER TABLE ONLY public."PhysicianRegion" DROP CONSTRAINT "PhysicianRegion_PhysicianId_fkey";
       public          postgres    false    247    243    4925            w           2606    33089 -   PhysicianRegion PhysicianRegion_RegionId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."PhysicianRegion"
    ADD CONSTRAINT "PhysicianRegion_RegionId_fkey" FOREIGN KEY ("RegionId") REFERENCES public."Region"("RegionId");
 [   ALTER TABLE ONLY public."PhysicianRegion" DROP CONSTRAINT "PhysicianRegion_RegionId_fkey";
       public          postgres    false    227    4907    247            r           2606    33055 "   Physician Physician_CreatedBy_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Physician"
    ADD CONSTRAINT "Physician_CreatedBy_fkey" FOREIGN KEY ("CreatedBy") REFERENCES public."AspNetUsers"("Id");
 P   ALTER TABLE ONLY public."Physician" DROP CONSTRAINT "Physician_CreatedBy_fkey";
       public          postgres    false    243    4893    215            s           2606    33050    Physician Physician_Id_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Physician"
    ADD CONSTRAINT "Physician_Id_fkey" FOREIGN KEY ("Id") REFERENCES public."AspNetUsers"("Id");
 I   ALTER TABLE ONLY public."Physician" DROP CONSTRAINT "Physician_Id_fkey";
       public          postgres    false    4893    215    243            t           2606    33060 #   Physician Physician_ModifiedBy_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Physician"
    ADD CONSTRAINT "Physician_ModifiedBy_fkey" FOREIGN KEY ("ModifiedBy") REFERENCES public."AspNetUsers"("Id");
 Q   ALTER TABLE ONLY public."Physician" DROP CONSTRAINT "Physician_ModifiedBy_fkey";
       public          postgres    false    243    4893    215            z           2606    33127 /   RequestBusiness RequestBusiness_BusinessId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestBusiness"
    ADD CONSTRAINT "RequestBusiness_BusinessId_fkey" FOREIGN KEY ("BusinessId") REFERENCES public."Business"("BusinessId");
 ]   ALTER TABLE ONLY public."RequestBusiness" DROP CONSTRAINT "RequestBusiness_BusinessId_fkey";
       public          postgres    false    4911    251    231            {           2606    33122 .   RequestBusiness RequestBusiness_RequestId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestBusiness"
    ADD CONSTRAINT "RequestBusiness_RequestId_fkey" FOREIGN KEY ("RequestId") REFERENCES public."Request"("RequestId");
 \   ALTER TABLE ONLY public."RequestBusiness" DROP CONSTRAINT "RequestBusiness_RequestId_fkey";
       public          postgres    false    4931    251    249            |           2606    33146 )   RequestClient RequestClient_RegionId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestClient"
    ADD CONSTRAINT "RequestClient_RegionId_fkey" FOREIGN KEY ("RegionId") REFERENCES public."Region"("RegionId");
 W   ALTER TABLE ONLY public."RequestClient" DROP CONSTRAINT "RequestClient_RegionId_fkey";
       public          postgres    false    227    4907    253            }           2606    33141 *   RequestClient RequestClient_RequestId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestClient"
    ADD CONSTRAINT "RequestClient_RequestId_fkey" FOREIGN KEY ("RequestId") REFERENCES public."Request"("RequestId");
 X   ALTER TABLE ONLY public."RequestClient" DROP CONSTRAINT "RequestClient_RequestId_fkey";
       public          postgres    false    249    4931    253            �           2606    33189 *   RequestClosed RequestClosed_RequestId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestClosed"
    ADD CONSTRAINT "RequestClosed_RequestId_fkey" FOREIGN KEY ("RequestId") REFERENCES public."Request"("RequestId");
 X   ALTER TABLE ONLY public."RequestClosed" DROP CONSTRAINT "RequestClosed_RequestId_fkey";
       public          postgres    false    4931    257    249            �           2606    33194 3   RequestClosed RequestClosed_RequestStatusLogId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestClosed"
    ADD CONSTRAINT "RequestClosed_RequestStatusLogId_fkey" FOREIGN KEY ("RequestStatusLogId") REFERENCES public."RequestStatusLog"("RequestStatusLogId");
 a   ALTER TABLE ONLY public."RequestClosed" DROP CONSTRAINT "RequestClosed_RequestStatusLogId_fkey";
       public          postgres    false    255    257    4937            �           2606    33211 2   RequestConcierge RequestConcierge_ConciergeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestConcierge"
    ADD CONSTRAINT "RequestConcierge_ConciergeId_fkey" FOREIGN KEY ("ConciergeId") REFERENCES public."Concierge"("ConciergeId");
 `   ALTER TABLE ONLY public."RequestConcierge" DROP CONSTRAINT "RequestConcierge_ConciergeId_fkey";
       public          postgres    false    233    259    4913            �           2606    33206 0   RequestConcierge RequestConcierge_RequestId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestConcierge"
    ADD CONSTRAINT "RequestConcierge_RequestId_fkey" FOREIGN KEY ("RequestId") REFERENCES public."Request"("RequestId");
 ^   ALTER TABLE ONLY public."RequestConcierge" DROP CONSTRAINT "RequestConcierge_RequestId_fkey";
       public          postgres    false    4931    259    249            �           2606    33230 (   RequestNotes RequestNotes_CreatedBy_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestNotes"
    ADD CONSTRAINT "RequestNotes_CreatedBy_fkey" FOREIGN KEY ("CreatedBy") REFERENCES public."AspNetUsers"("Id");
 V   ALTER TABLE ONLY public."RequestNotes" DROP CONSTRAINT "RequestNotes_CreatedBy_fkey";
       public          postgres    false    215    4893    261            �           2606    33235 )   RequestNotes RequestNotes_ModifiedBy_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestNotes"
    ADD CONSTRAINT "RequestNotes_ModifiedBy_fkey" FOREIGN KEY ("ModifiedBy") REFERENCES public."AspNetUsers"("Id");
 W   ALTER TABLE ONLY public."RequestNotes" DROP CONSTRAINT "RequestNotes_ModifiedBy_fkey";
       public          postgres    false    4893    261    215            �           2606    33225 (   RequestNotes RequestNotes_RequestId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestNotes"
    ADD CONSTRAINT "RequestNotes_RequestId_fkey" FOREIGN KEY ("RequestId") REFERENCES public."Request"("RequestId");
 V   ALTER TABLE ONLY public."RequestNotes" DROP CONSTRAINT "RequestNotes_RequestId_fkey";
       public          postgres    false    4931    261    249            ~           2606    33170 .   RequestStatusLog RequestStatusLog_AdminId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestStatusLog"
    ADD CONSTRAINT "RequestStatusLog_AdminId_fkey" FOREIGN KEY ("AdminId") REFERENCES public."Admin"("AdminId");
 \   ALTER TABLE ONLY public."RequestStatusLog" DROP CONSTRAINT "RequestStatusLog_AdminId_fkey";
       public          postgres    false    218    255    4897                       2606    33165 2   RequestStatusLog RequestStatusLog_PhysicianId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestStatusLog"
    ADD CONSTRAINT "RequestStatusLog_PhysicianId_fkey" FOREIGN KEY ("PhysicianId") REFERENCES public."Physician"("PhysicianId");
 `   ALTER TABLE ONLY public."RequestStatusLog" DROP CONSTRAINT "RequestStatusLog_PhysicianId_fkey";
       public          postgres    false    243    4925    255            �           2606    33160 0   RequestStatusLog RequestStatusLog_RequestId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestStatusLog"
    ADD CONSTRAINT "RequestStatusLog_RequestId_fkey" FOREIGN KEY ("RequestId") REFERENCES public."Request"("RequestId");
 ^   ALTER TABLE ONLY public."RequestStatusLog" DROP CONSTRAINT "RequestStatusLog_RequestId_fkey";
       public          postgres    false    249    255    4931            �           2606    33175 9   RequestStatusLog RequestStatusLog_TransToPhysicianId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestStatusLog"
    ADD CONSTRAINT "RequestStatusLog_TransToPhysicianId_fkey" FOREIGN KEY ("TransToPhysicianId") REFERENCES public."Physician"("PhysicianId");
 g   ALTER TABLE ONLY public."RequestStatusLog" DROP CONSTRAINT "RequestStatusLog_TransToPhysicianId_fkey";
       public          postgres    false    243    4925    255            �           2606    33266 ,   RequestWiseFile RequestWiseFile_AdminId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestWiseFile"
    ADD CONSTRAINT "RequestWiseFile_AdminId_fkey" FOREIGN KEY ("AdminId") REFERENCES public."Admin"("AdminId");
 Z   ALTER TABLE ONLY public."RequestWiseFile" DROP CONSTRAINT "RequestWiseFile_AdminId_fkey";
       public          postgres    false    265    218    4897            �           2606    33261 0   RequestWiseFile RequestWiseFile_PhysicianId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestWiseFile"
    ADD CONSTRAINT "RequestWiseFile_PhysicianId_fkey" FOREIGN KEY ("PhysicianId") REFERENCES public."Physician"("PhysicianId");
 ^   ALTER TABLE ONLY public."RequestWiseFile" DROP CONSTRAINT "RequestWiseFile_PhysicianId_fkey";
       public          postgres    false    265    243    4925            �           2606    33256 .   RequestWiseFile RequestWiseFile_RequestId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RequestWiseFile"
    ADD CONSTRAINT "RequestWiseFile_RequestId_fkey" FOREIGN KEY ("RequestId") REFERENCES public."Request"("RequestId");
 \   ALTER TABLE ONLY public."RequestWiseFile" DROP CONSTRAINT "RequestWiseFile_RequestId_fkey";
       public          postgres    false    4931    249    265            x           2606    33110     Request Request_PhysicianId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Request"
    ADD CONSTRAINT "Request_PhysicianId_fkey" FOREIGN KEY ("PhysicianId") REFERENCES public."Physician"("PhysicianId");
 N   ALTER TABLE ONLY public."Request" DROP CONSTRAINT "Request_PhysicianId_fkey";
       public          postgres    false    4925    243    249            y           2606    33105    Request Request_UserId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Request"
    ADD CONSTRAINT "Request_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES public."User"("UserId");
 I   ALTER TABLE ONLY public."Request" DROP CONSTRAINT "Request_UserId_fkey";
       public          postgres    false    249    4915    234            �           2606    33299    RoleMenu RoleMenu_MenuId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RoleMenu"
    ADD CONSTRAINT "RoleMenu_MenuId_fkey" FOREIGN KEY ("MenuId") REFERENCES public."Menu"("MenuId");
 K   ALTER TABLE ONLY public."RoleMenu" DROP CONSTRAINT "RoleMenu_MenuId_fkey";
       public          postgres    false    271    269    4951            �           2606    33294    RoleMenu RoleMenu_RoleId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."RoleMenu"
    ADD CONSTRAINT "RoleMenu_RoleId_fkey" FOREIGN KEY ("RoleId") REFERENCES public."Role"("RoleId");
 K   ALTER TABLE ONLY public."RoleMenu" DROP CONSTRAINT "RoleMenu_RoleId_fkey";
       public          postgres    false    271    4949    267            �           2606    33350 1   ShiftDetailRegion ShiftDetailRegion_RegionId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."ShiftDetailRegion"
    ADD CONSTRAINT "ShiftDetailRegion_RegionId_fkey" FOREIGN KEY ("RegionId") REFERENCES public."Region"("RegionId");
 _   ALTER TABLE ONLY public."ShiftDetailRegion" DROP CONSTRAINT "ShiftDetailRegion_RegionId_fkey";
       public          postgres    false    227    4907    277            �           2606    33345 6   ShiftDetailRegion ShiftDetailRegion_ShiftDetailId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."ShiftDetailRegion"
    ADD CONSTRAINT "ShiftDetailRegion_ShiftDetailId_fkey" FOREIGN KEY ("ShiftDetailId") REFERENCES public."ShiftDetail"("ShiftDetailId");
 d   ALTER TABLE ONLY public."ShiftDetailRegion" DROP CONSTRAINT "ShiftDetailRegion_ShiftDetailId_fkey";
       public          postgres    false    277    4957    275            �           2606    33333 '   ShiftDetail ShiftDetail_ModifiedBy_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."ShiftDetail"
    ADD CONSTRAINT "ShiftDetail_ModifiedBy_fkey" FOREIGN KEY ("ModifiedBy") REFERENCES public."AspNetUsers"("Id");
 U   ALTER TABLE ONLY public."ShiftDetail" DROP CONSTRAINT "ShiftDetail_ModifiedBy_fkey";
       public          postgres    false    215    275    4893            �           2606    33328 $   ShiftDetail ShiftDetail_ShiftId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."ShiftDetail"
    ADD CONSTRAINT "ShiftDetail_ShiftId_fkey" FOREIGN KEY ("ShiftId") REFERENCES public."Shift"("ShiftId");
 R   ALTER TABLE ONLY public."ShiftDetail" DROP CONSTRAINT "ShiftDetail_ShiftId_fkey";
       public          postgres    false    273    275    4955            �           2606    33316    Shift Shift_CreatedBy_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Shift"
    ADD CONSTRAINT "Shift_CreatedBy_fkey" FOREIGN KEY ("CreatedBy") REFERENCES public."AspNetUsers"("Id");
 H   ALTER TABLE ONLY public."Shift" DROP CONSTRAINT "Shift_CreatedBy_fkey";
       public          postgres    false    4893    273    215            �           2606    33311    Shift Shift_PhysicianId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Shift"
    ADD CONSTRAINT "Shift_PhysicianId_fkey" FOREIGN KEY ("PhysicianId") REFERENCES public."Physician"("PhysicianId");
 J   ALTER TABLE ONLY public."Shift" DROP CONSTRAINT "Shift_PhysicianId_fkey";
       public          postgres    false    4925    273    243            o           2606    32989    User User_Id_fkey    FK CONSTRAINT     {   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_Id_fkey" FOREIGN KEY ("Id") REFERENCES public."AspNetUsers"("Id");
 ?   ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_Id_fkey";
       public          postgres    false    234    215    4893            p           2606    32999    User User_ModifiedBy_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_ModifiedBy_fkey" FOREIGN KEY ("ModifiedBy") REFERENCES public."AspNetUsers"("Id");
 G   ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_ModifiedBy_fkey";
       public          postgres    false    4893    215    234            &   V   x�3�LL����$�s3s���s9ML��-,��8�K�RSK��grfI�!g�T�1����X�����$���b���� ��"x      1      x������ � �      $      x������ � �      '      x������ � �      #   v  x��U�n9<�|�@Q"%�O^`9�r��3�g���/�mĎ�~`�hɪ�rI6�sF�$�P�d�`���rm�S���>��}����.���p=��	�G
��N߾N�:4�/ .�..�� ��]l%�.Ƀ���I�ES|�PRh0n�i,�)�D,����,� έOw7�s�O�����P�4mrۼ�]8>��X6�^X����T"@=�b�#�XLc��F׬s���F�vO�sd� x�9/��9x������hr�Fs�WZn�j��M_گ�Èt��O����]��=��ӂ<K���"H���`��p$2-�\�x��nڶ���t�Oo"������st�T�A��*�:�n42�ڢ䆡D�������"Ա>��= ��{�US�uO%�*@U�&S�J��:��hZ�QR���Wy6�yf������M�+��k�����B:��)�\��7��)C�/�fO����:{�W2�T�XU�xT� [��IE5<��ym�X-�/����!"�FI�lU��e�6��uH�����(��i?֟�W�������	�۸�Fk�������ae�t.��zh�֎�k�|i�6�e'�ݝ�m1�DA�po���kb���1ݯ"��	G��6�, 3�@q���p�^�?�[�ИϦ!�`��5QB�9+��PUV:L�1N:K�B���BTW��n�^"�
��#eu[�jzR"�|O��	�:m"�B�a�k���׈�����L5>B2����fK5It�#�֧�kH�w��ӝ"�]��wo� �Q6�K�����	UU�F��،��k���XT�-��4�Hc3�*Zp*�4&�ө_��w"��k���]V�擷�Z_�1���j�o�n��8��      )      x������ � �      3   W   x�3��LIOI���@d�!�1%73�����D��H��\��������L������U��%�FZ*XZ���������	��1z\\\ {�&�      +      x������ � �      -      x������ � �      5   �   x��ѽN�0��y
�@����sb``b`�bH�j%-}}�6@���%�����Q��ؼ�Mޤ~���7��u��ͭ�Z��Z8�hA$�G0��l���B��z܏�Ќ���3��ڙ�.T�B =��|6����Yty�gA�Ѐ��}ö��ᚰ����X!�v4�^�5��^�v���ПS��Bҥ���Ҭ�H�ޱ����y~��%�ȶ�����4�J�k����p,7�����\�TUU�츳�      7      x������ � �      9      x������ � �      ;      x������ � �      Y      x������ � �      =      x������ � �      ?   w   x�3�,�(J�L�-Ȩ,�L�L̃
pz'�%�erg;��&f��%��r����[Xr���'��p���#CN#�:������*ld``�k`�k`�```FPc�@� �80����� ys,      d      x������ � �      A      x������ � �      C      x������ � �      /      x�3�LLJ�������  �      E   4  x��V�n�0<���?b��%O>��S=���!	v�@Ic��KR��:���!H�1�ٙ��2 ���}��q�`�qߝ#�T�X�/��U���X����H���tKrAr�,�q�d���ZHeA%6~n_�:��v}�<�e%�Kh��0QSLizMuL��Čx�_����^O�S0V�4r����C�p3qE��#]AyI0-Lڠ�U�F��&����n�?�%9�H���:���d�WN�Ғ6��#��#�7Cd8��_�ԤK���BJ򕞠�a�f�̍�/�mSH2��N.�X��K/��8�Ր�<_�?��r�A�`H(6�u9��������ݜI}�����Ҧ!W�;(=�<�>��:���x;�qo�K/�B2z�䂥��;�y�]����1�avu��%�NB�|����������}�����+�[��4��w���mV���'9p1�U�*(�ps����6��c�U/�wFV�X�e!�nv���R�5�MZ3���i�In�'�_s�{�T7����S����8�N�4	��"���*�G�X,��P$�      G      x������ � �      I   Y  x���K��0���)8  ?BHv�ԋY�	zSĉ�&1#'����I�ϴbd�T\_=���9��ϲ;Xx3�itr��L�,��+,>�ŮV`��Z7h�E�k�W�RG�[Tƶf*���vׁ���d�K���^��d�Ĝ����p@����s�����0U���U�����y�'�~Զ���D��){����#�y����}Y���:{mζkb��+Z
06B�I�3����+kȳUF�/������C���d���4�;T��[�������[�3��5ni�wl�����^C��@$���<C�^þ-=i���X냃!�q,
�J�M�5k�,ܨڢܧ�>���x+n�TF��[d
*�G���J"%�bIr#qF*}�d�9/������I��T���B��զ�g%}{F�Z�4����W8��@Qpg���1�A�|JV����;����kL�_����gULJRI����T���KcܭKe������$g�p��dMFU�ҟLt�
a�D;���3��1��N�SŒ���뫌蜙n�'�����Y���*J#bE�犎^��r��Vpr�L& �?P      M      x������ � �      O      x������ � �      Q   �   x�m�K��0�ur�^��호�!8��Bx�"�Fs{�E$�"��y6�����_3]�?�{3,�1ͻ���8���� ���LZ1�AY�;����t���3pާ�!���x�!��S��������U��(w::n+ݥ�3�xm@6D����E�X>j5f���&�p�Z�2��2��i�W.Y��{F'��؍��> �J{�      K   =   x�3�42��4�?N�"=G'g��������N##]#]#K+0)������� ��       S   7   x�3�H,�L�+�2�tK��̩�2�t��K�L-JO�2�t*-��K-.����� W#�      U   �  x���ъ�0E�������HIo!�>�Pڷ�ō�����w��Q �
���s.�;}�d���c���[?��^~���Q�q����޴jY�տ�]���d��'p�b���K~������"rY6E�&u�� 
�M�ٔEAD��y��J��i!�K�l�/Ң�b��6c�۱����!�K$��8�S�5��7=���l���� ��>���rP�˚�˹�if��)�8�Ԡ�q���i~S���f��.)�d��To�II�TL*4�CM�c�"�
�2s��0�ң�:�:�kߎ������tj�E��O̍8N�ioM��ǉ����He/�"�[�(3������������7�����l��#���Rm��}��_�=w@Y���Z�˱��t�mӵ��k)wX��.9���~ֻ��#�(�      W      x������ � �      [      x������ � �      b      x������ � �      ]      x������ � �      _      x������ � �      a      x������ � �      6   �  x����r�8���S���n46����4O��ERd�)�K���!z�Ţ��������=*\�A�S6Q3�&Эe�qr��_��a���nw�M:����q��I��!�ި股I}�[�MSۦ6�(�b?7�_����5��p�0R-��_�|^��h�.*,ѻ�IG�Q�䛐O�K�q�첏Q8���k-i���T�iӔ�7|3�/�Gv#Ӏ�E�K�n���m�m��B�\tʆ57 Z�R�k��lgD$�ց񵠧�Ve�b�֏�-.!A4
S-@�u�I3���HM#[y���h����o��tW�/���.J:�����L4K�(��\u������5��1g�rz��ή�v�ر���c�� � ,�"��g�-3ݒx�C�:�M�!��>�,Ýy��m��Z�J/�w���f���'�8)8?����'�*L�B.:��5��K����0���Ǿ���'�o�?k3C�m�9�%.ќe�����֑����&1IA����\���~�ۋ�-��Oe�;m��:����;������N�� ��
�
�HnD;F���
be'�g���H琌d��LS��5������m:[s�}��V�A6_�W�Q�h1z�ಚr�$�1���Sh�m��d�S_�w�g�d޿�{q&50Z�B���@m��V��S�h1im�:%6�܊�\E���Hǚ�<��[[<y8����Fi�Υ���b���b��_�"�װX�H��`!��
Dj|b��}:xa>���v�=��iw���x�ʯ&���H�k�,8��]k�R��c��8xɇ�]k�>�:Uc%**�����6��i/?��3�8�;rbD\l}����N
TG�z��T'xk��5�GDc���O=����p3>����	T����>���>�C�`�!���U��C1^fP�P36{]$25KU�ɶ�)�6a�I�O_�iH/��H���w�Ȗ�#��۰Z�����     