# FuelTracker
An application to easily collect fuel consumption statistics for analysis in external programs.

Minimum version: Android 6 (Marshmallow).
Targeted version: Android 10 (Q)
Tested on: Nokia 6.1 running Android 10.

# Usage
- In the app you can save the details of fuel refills.
- You can then export (share) the data as a json file to some place conveinient. (I'd recommend a dedicated folder in google drive/onedrive for this.)
- Install desktop client for the cloud-service of your choise and enable offline access for the specific folder. (Not mandatory but makes the process more conveinient.)
- Open the attached excel spreadsheet on your computer (Uses Power Query so might not work on excel versions older than 2016).
- Point the File path present on one of the cells to the locallly synced cloud folder.
- Refresh the data.
- (You may wish to hide the first entry as most of the calculations won't work for it. )

You can use this excel file for analyzing the data https://github.com/SpaceBeeGaming/FuelTracker/blob/master/FuelTrackerAnalysis.xlsx

# Screenshots

### App
![Entry list](Screenshots/App_List.png) 
![Add new entry dialog](Screenshots/App_Add.png) 
![Entry details](Screenshots/App_Details.png)

### Analysis spreadsheet:
![Excel analysis](Screenshots/Excel_Analysis.png)
