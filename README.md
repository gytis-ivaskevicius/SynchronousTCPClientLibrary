# SynchronousTCPClientLibrary
Simple Synchronous TCP Client Library written in C#.


# Documentation

<br>

## connectFromNewThread(string address, (int or string) port)
### Description:
Creates a new thread and runs "connect" method from it.
### Parameters
1. address - Ip address. (In string format)
2. port - Port. (In string or int format)

<br>

## connect(string address, (int or string) port)
### Description:
Connects to the pecified server, over specified port. 
### Note:
Runs within the same thread. In other words - everything that goes after this line will not be called untill it disconnects.
### Parameters
1. address - Ip address. (In string format)
2. port - Port. (In string or int format)

<br>

## send(byte[] bytes)
### Description:
Sends specified bytes.
### Parameters
1. message - Array of bytes.

<br>

## send(string message)
### Description:
Sends string message to the server.
### Parameters
1. message - basic string message.

<br>

## sendln(string message)
### Description:
Sends string message(with newLine symbol at the end) to the server.
### Parameters
1. message - basic string message.

<br>

## stopClient()
### Description:
Stops client

<br>

## Connected - getter
### Description:
Returns true if client is running.









