# LocalSNMP

LocalSNMP is app made to monitor local network. It's using #SNMP library.

Configuration:
In ScanService you can specify netowrk address, start range and close range. (default it's 192.168.1.0, 1, 254 so it will go through 192.168.1.1 - 192.169.1.254)

There are 2 accounts seed:
  "admin" with password "password"
  "user" with password "password"
  To authorize EP you have to set value of "Authorization" header to "bearer [TOKEN]" where [TOKEN] is response from login EP
  
Scanning is occuring every hour(you can change that) in background so endpoints can be reached at anytime.
