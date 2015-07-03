# turtleALM

## Purpose
TurtleALM is an Issue Tracker plugin for TortoiseSVN, TortoiseGIT, TortoiseCVS and TortoiseHG in support of [HP ALM](http://hp.com/go/alm)

TurtleALM allows to quickly write meaningful commit messages

TurtleALM is implemented as a Bugtraq provider for TortoiseSVN/CVS/GIT/HG. It has successfully been tested with TortoiseSVN and TortoiseGIT. Feedback and help is appreciated for the support of other Tortoise versions.

TurtleALM works with HP ALM11 onward (leverages the ALM REST API). There is no need for the ALM OTA connectivity add-in.

## Download
Download TurtleALM installer [here]().

## Configure
Tortoise configuration
The installer adds TurtleALM as a plugin for TortoiseSVN automatically. There is only 
one configuration item in Tortoise: the path. This path entry indicates which plugin 
to use for which directory. If your source code is on C:\ drive, you need to put "C:\". 
Refer to the Tortoise manual for more details.

## TurtleALM configuration
In addition, TurtleALM uses the Windows registry to customize its configuration. 
This is all located in HKEY_CURRENT_USER\Software\TurtleALM. It is
then very easy to create a .reg file which you can provide to all your 
colleagues to match a given configuration.

You can tune the following parameters:
* lastQCURL: the last ALM URL used
* QCURLs: list of ALM farms, separated by commas. Example: “http://myalm/qcbin”
* DefectPrefix: used to construct the commit message (see useGUID below)
* ReqPrefix: for future use
* Verb: verb used before each defect when constructing the commit message
* useGUID
 * False: the commit message is constructed with this format:
[verb] [DefectPrefix]:ALMDomain:ALMProject:id - summary
 * True: the commit message is constructed with this format:
[verb] [value of GUID field] - summary
Default: False

* GUIDDefectField: name of the field to be used as GUID (global unique ID) field. Example: "user-27"
* GUIDReqField: name of the field to be used as GUID (global unique ID) field. Example: "user-23"

# Use
From the commit window, click “choose QC/ALM item” button.



From there, connect to your ALM project. TurtleALM will list all defects that are not closed and assigned to you. Choose the one which pertains to this code change. You can select several items, from several projects. Click


on the column headers to sort the items.



Click “OK” to have the commit message added automatically for you.



# FAQ
* Q: Why did you named it "TurtleALM"?
 * A: The Tortoise project highly recommends naming conventions. We just did!

* Q: Does it work with QC10 and below?
 * A: No – only QC/ALM 11 – and above, through the REST API, in order to remove a dependency on 32 bits and QC OTA API. It should be much easier to configure and setup for users.

* Q: TurtleALM allows to save my credentials – is it safe?
 * A: You judge. It is using Microsoft Cryptography API – Reference: http://msdn.microsoft.com/en-us/library/ms995355.aspx#windataprotection-dpapi_topic04
