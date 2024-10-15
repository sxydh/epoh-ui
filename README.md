# What is EpohUI

EpohUI is a cross-platform framework implemented in C#, designed to integrate the capabilities of browsers and system interfaces.

## What Can EpohUI Do

* Lightweight HTTP Server  
  EpohUI is primarily a minimal HTTP server, allowing you to browse the application’s directories and files.  
  This means you can directly deploy web applications within the app, such as Vue, React, Angular, or even just write a native HTML file.  
  
* Call System Interfaces  
  EpohUI allows you to call system interfaces via HTTP protocol, such as IO, Net, Database, Thread, Process, etc.  
  
* Call Custom Interfaces  
  In addition to calling system interfaces, EpohUI can dynamically invoke user-defined interfaces without the need to compile or package the entire application.  

## How to Use EpohUI  

Using EpohUI is very simple, just one step:  

* Place EpohUI.Core.exe in the folder containing index.html, and run EpohUI.Core.exe to access the application homepage.  

If you need to call system interfaces, it's also very easy:  

```JavaScript
// The path lib/hello-world is the unique identifier for the system interface
fetch("http://localhost:33/api/lib/hello-world")
  .then(res => res.text())
  .then(data => {
    // Here you can get the return value of the system interface
    console.log(data);
  });
```

If you need to call custom interfaces, just follow these two steps:  

* Copy your user DLL files into the DLLs folder.
* Call via HTTP.

## Current and Future Capabilities of EpohUI

* \[TODO\] File Operations
* \[TODO\] Database Operations
* \[TODO\] Process Operations
* \[TODO\] Multithreading Tasks
* \[TODO\] Security
* \[TODO\] Cross-Platform

## EpohUI Architecture

TODO
