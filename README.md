# Fileserver

Simple fileshareing system for users to share large files with someone for a short time.  
Use cases:
- Make it easy for people to share family photos ++ with familymembers
- Small businesses to share project file with a third party vendor.

## To implement:
- [x] ~~Login~~
- [ ] Register
- [ ] Role based access
- [x] ~~Possibillity to create a folder~~
- [ ] Possibillity to set a password for fileupload by unauthenticated users
- [x] ~~Upload files~~
- [ ] Delete files
- [x] ~~Download files~~
- [x] ~~Preview files (partially)~~

## Future work:
- [ ] Set expiration for a folder
- [ ] Delete folder after x amount of time after expiration
- [ ] Let users reactivate folder before it is deleted
- [ ] Payment per folder based on folder size and length to expiration (different tiers?)
- [ ] Folder max size
- [ ] Full administration panel (features?)
- [ ] Mailsender for account verification and notification about folder expiration and deletion
- [ ] Make all userinput fields stripped of potentially dangerous text
- [ ] Make sure fileSafety is good enough (Do not save file to application folder, do not allow file to be executed by server process ++)
