﻿select substring(Image_Date,1,10) as Ah from UploadImage where User_Id='c3f0270b-3b1d-470b-9e85-a91e091371e4' UNION select substring(Thought_Date, 1, 10) as Ah from UploadThoughts where User_Id='c3f0270b-3b1d-470b-9e85-a91e091371e4' order by Ah desc;
select Thought_Text, substring(Thought_Date,1,10) from UploadThoughts where User_Id ='c3f0270b-3b1d-470b-9e85-a91e091371e4' order by Thought_Date desc;
select count(Thought_Id) from UploadThoughts where '28/10/2016'=substring(Thought_Date,1,10) and User_Id='c3f0270b-3b1d-470b-9e85-a91e091371e4';