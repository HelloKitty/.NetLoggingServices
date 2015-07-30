# .Net 4.5 Logging Services

.Net 4.5 and Mono logging services for any application.

##Why?

This repo is aimed at producing a portable library for logging targeting .Net 4.5 and Mono. More importantly, it exists to provide a unified application agnostic logging service with general logging data such as caller and importance level. The idea is to provide an extendable API for logging while also keeping message logging consistent various projects.

##How to Use

There are many ways to utilize this library for logging messages in .Net.

- Implement [ILogger](LoggingServices/Logging/Loggers/ILogger.cs) on a logging service.

- Inherit from [ThreadedLogger](LoggingServices/Logging/ThreadedLogger.cs) and [ILogger](LoggingServices/Logging/Loggers/ILogger.cs) on a logging service to handle and log messages off the main thread.

- Utilize the prewritten [ILogger](LoggingServices/Logging/Loggers/ILogger.cs)s in the library.

##In Progress

- A semi-functional chain logger class exists that can, at construction time, be supplied various [ILogger](LoggingServices/Logging/Loggers/ILogger.cs)s. However, the functionality to allow for registering and unregistering of loggers has yet to be implemented.

- Email/Text [ILogger](LoggingServices/Logging/Loggers/ILogger.cs) is partially implemented based on [this](https://msdn.microsoft.com/en-us/library/system.net.mail.smtpclient(v=vs.110).aspx) .Net SMTP class.

- Various other [ILogger](LoggingServices/Logging/Loggers/ILogger.cs)s that write to other locations such as; Database, flat file, sockets and more.


#Build

Windows: Verified Locally

Linux/Mono: [![Build Status](https://travis-ci.org/HelloKitty/.NetLoggingServices.svg?branch=master)](https://travis-ci.org/HelloKitty/.NetLoggingServices)
