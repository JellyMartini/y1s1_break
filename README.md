# y1s1_break
Open repo for the First Year First Semester game

Upon opening a scene, set the Layer of the Player GameObject to Player, and Debug objects to IgnoreRaycast
If the scene does not contain a Player Layer, create one

Textures will be stored in a collaborative OneDrive folder, not the git repo, so that updating remote repos is smooth

**DO NOT DELETE .keep**

# How to Get
Create an empty Unity Project. Name it whatever you want.
If the project opens, close it.
Open up the new project's folder in File Explorer. Click the bar left of the search bar and copy the highlighted text. This copies the folder path.
Delete the Assets folder.
Open the Windows Powershell Application and type `cd`, then a space, then the folder path you copied earlier. Hit enter.
The line should now start with `PS copied-path> `
Copy and paste `git clone https://github.com/JellyMartini/y1s1_break.git`. Hit enter.
Go back to the project's folder in File Explorer. There should be a new folder called "y1s1_break". Rename it to Assets.
Open the Unity Project through the Unity Hub.
Open the Scene you want to use.
All done!

# How to Start Editing
Open the Assets folder in File Explorer.
Copy the folder path.
In Windows Powershell, type `cd copied-folder-path`. Hit enter.
Type `git pull`. Hit enter.
You will be creating a "branch", a copy of the repo that can be edited outside of the main branch.
Decide on a name for the branch, like your name.
In the same Powershell terminal, type `git checkout -b branchname`. Hit enter.
Now type `git push --set-upstream origin branchname`.
Now you can do whatever you want, and it won't affect other people's edits!

# How to Continue Editing
Open the Assets folder in File Explorer. Copy the folder path.
In Powershell, type `cd copied-folder-path`. Hit enter.
Now type `git status`. It will spit back `On branch ` and the branch name.
If you are on the wrong branch, type `git checkout branchname`. Hit enter.
Type `git pull` and hit enter.

# How to Save Edits
Open the Assets folder in File Explorer. Copy the folder path.
In Powershell, type `cd copied-folder-path`. Hit enter.
Now type `git add .`. Hit enter.
Now you have to decide what the note for your save will be. This is your commitnote.
Now type `git commit -m "commitnote"`. Don't forget the quotation marks around your note. Hit enter.
Now type `git push`. Hit enter.
If it now tells you to do `git push --set-upstream origin branchname`, do that.

# How to get your Edits into the Main branch
Open a pull request on GitHub.
Fill it out with the required info. I'll review it to make sure nothing will break. 
If it's all good, I'll merge the branches and delete your edit branch.
After the merge, you will have to type `git checkout main`, hit enter, then `git branch -d branchname`, and hit enter.
Now type `git pull`.
