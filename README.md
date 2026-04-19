<img width="640" height="174" alt="image" src="https://github.com/user-attachments/assets/726d7ff6-0660-4f6b-97d8-0d89f8a1e99f" />

Introduction
------------

This repository implements a network diagnostic tool based on `Ping` class located in the `System.Net.NetworkInformation` namespace. The tool can be used for different scenarios such as host monitoring, tracing routes and scanning range of IP addresses. In addition to these network tools, the application also tracks statistics about tracked hosts and allows the user to display those statistics as charts.

Ping Class
----------

The most common way to ping another host is to send ICMP \[[RFC792](http://www.faqs.org/rfcs/rfc792.html)\] echo message and then wait for the response from the targeted host. As it was stated at the beginning of the article, you can use `Ping` class available in .NET Framework that wraps this type of ICMP message to make your life easier and save yourself from implementing the protocol by yourself.

`Ping` class goes together with some other classes and data types such as `PingOptions` and `PingReply` classes and `IPStatus` enumeration. The heart of the class is the `Send` method \[and `SendAsync`\] that sends echo message and waits for the response. The method has several overloads, but all of them return an instance of the `PingReply` class which contains results of echo request \[`Status` property\] and response time \[`RoundtripTime`\]. Here are a few examples:

```csharp
Ping pinger = new Ping();

// first parameter is TTL and second sets flag in IP header to 
// tell routers not to fragment the datagram
PingReply reply = PingOptions pingerOptions = new PingOptions(127, false); 

// specify computer by its name
PingReply reply = pinger.Send("localhost");

// with timeout specified
PingReply reply = pinger.Send("localhost", 1000); 

// specify computer by its IP address
PingReply reply = pinger.Send(new IPAddress(new byte[] { 127, 0, 0, 1 }));

// specify computer by its name, sets timeout, uses user defined buffer and 
// options
PingReply reply = pinger.Send(new IPAddress(new byte[] { 127, 0, 0, 1 }),
    1000, buffer, pingerOptions);
```

But it is only one half of the story. Based on this simple method, we need to provide some meaningful and useful statistical information.

HostPinger Class
----------------

`HostPinger` class is built on top of the `Ping` class and it is in charge of pinging the host and calculating, storing and providing statistics about it.

The class has five `public` constructors:

```csharp
// constructs pinger for localhost (127.0.0.1)
public HostPinger()

// constructs pinger from XML config file
public HostPinger(XmlNode node)

// use DNS to resolve the IP address from hostname
public HostPinger(string hostName)

// construct pinger for host without a name
public HostPinger(IPAddress address)

// it the IP address specified, hostname can be used as a description
public HostPinger(string hostName, IPAddress address);
```

Pinging is controlled with `Start` and `Stop` methods or by setting the `IsRunning` property. `HostPinger` class has four events \[their names describe their purpose well, so there is no need for additional explanations\]:

*   `OnPing`
*   `OnStatusChange`
*   `OnStartPinging`
*   `OnStopPinging`
*   `OnHostNameChanged`

User can also provide a logger to log some of these events \[`OnPing` and `OnHostNameChanged` events are not logged\] by setting `Logger` property. Provided logger must implement `IPingLogger` interface.

```csharp
public interface IPingLogger
{
    void LogStart(HostPinger host);
    void LogStop(HostPinger host);
    void LogStatusChange(HostPinger host, HostStatus oldStatus, HostStatus newStatus);
}
```

Host's statistics provided by this class are listed below:

| Name/Description                                                                     | Property Name                                                  | Data Type    | Values                                 |
|--------------------------------------------------------------------------------------|----------------------------------------------------------------|--------------|----------------------------------------|
| Host's current status                                                                | Status                                                         | `HostStatus` | `Alive`, `Dead`, `DnsError`, `Unknown` |
| Number of sent requests                                                              | SentPackets                                                    | `int`        | positive number                        |
| Number of received responses [successful pings]                                      | ReceivedPackets                                                | `int`        | positive number                        |
| Percent of received responses [successful pings]                                     | ReceivedPacketsPercent                                         | `float`      | (0, 100)                               |
| Number of lost packets [unsuccessful pings]                                          | LostPackets                                                    | `int`        | positive number                        |
| Percent of lost packets[successful pings]                                            | LostPacketsPercent                                             | `float`      | (0, 100)                               |
| Number of consecutively lost packets                                                 | ConsecutivePacketsLost                                         | `int`        | positive number                        |
| Maximal number of consecutively lost packets recorded                                | MaxConsecutivePacketsLost                                      | `int`        | positive number                        |
| Whether the last packet lost                                                         | LastPacketLost                                                 | `bool`       | true/false                             |
| Number of recently received responses [successful pings]                             | RecentlyReceivedPackets                                        | `int`        | positive number                        |
| Percent of recently received responses [successful pings]                            | RecentlyReceivedPacketsPercent                                 | `float`      | (0, 100)                               |
| Number of recently lost packets[unsuccessful pings]                                  | RecentlyLostPackets                                            | `int`        | positive number                        |
| Percent of recently lost packets[unsuccessful pings]                                 | RecentlyLostPacketsPercent                                     | `float`      | (0, 100)                               |
| Current response time of the host [in milliseconds]                                  | CurrentResponseTime                                            | `long`       | positive number                        |
| Average response time of the host [in milliseconds]                                  | AverageResponseTime                                            | `float`      | (0, ?)                                 |
| Shortest response time of the host [in milliseconds]                                 | MinResponseTime                                                | `long`       | positive number                        |
| Longest response time of the host [in milliseconds]                                  | MaxResponseTime                                                | `long`       | positive number                        |
| Duration of the current host's status                                                | GetStatusDuration method provides this information             | `TimeSpan`   |                                        |
| How long the host has been alive                                                     | GetStatusDuration method provides this information             | `TimeSpan`   |                                        |
| How long the host has been dead                                                      | GetStatusDuration method provides this information             | `TimeSpan`   |                                        |
| How long the pinger has not been able to obtain IP address of the host from its name | GetStatusDuration method provides this information             | `TimeSpan`   |                                        |
| How long the host status has been unknown                                            | GetStatusDuration method provides this information             | `TimeSpan`   |                                        |
| Availability of the host[in percents]                                                | HostAvailability                                               | `float`      | (0, 100)                               |
| Total duration of all ran tests                                                      | TotalTestDuration                                              | `TimeSpan`   |                                        |
| Duration of the current test                                                         | CurrentTestDuration                                            | `TimeSpan`   |                                        |

`ClearStatistics` method does as its name suggests, it clears all statistics, but offers a user the option to retain statistics that measure time durations.

And finally, the `Save` method will write ping options and host information using the provided `XmlWriter` object.

Host pinger can be configured with following options:

| Name of XML element in the configuration file that stores option                                                                                                   | Name of the property   | Type                | Required | Default Value        |
|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------|---------------------|----------|----------------------|
| name                                                                                                                                                               | `HostName`             | `string`            | Yes      | none                 |
| Host name. If IP address is not specified, pinger uses this name to query DNS server and obtain IP address of the host.                                            |
| ip                                                                                                                                                                 | `IPAddress`            | IP Address format   | No       | DNS query if not set |
| IP address of host. It is not a required field. If IP address is not specified, pinger uses this host name to query DNS server and obtain IP address of the host.  |
| description                                                                                                                                                        | `HostDescription`      | `string`            | No       | none                 |
| Description of the host.                                                                                                                                           |
| timeout                                                                                                                                                            | `Timeout`              | `int`               | No       | 2000                 |
| Time [milliseconds] to wait for each reply.                                                                                                                        |
| interval                                                                                                                                                           | `PingInterval`         | `int`               | No       | 1000                 |
| Time [milliseconds] that pinger waits after it receives reply from previous ping and before it sends another ping.                                                 |
| pingsbeforedead                                                                                                                                                    | `PingsBeforeDead`      | `int`               | No       | 10                   |
| Number of unsuccessful pings before pinger declares host as dead.                                                                                                  |
| buffersize                                                                                                                                                         | `BufferSize`           | `int`               | No       | 32                   |
| Size [bytes] of echo message.                                                                                                                                      |
| ttl                                                                                                                                                                | `TTL`                  | `int`               | No       | 32                   |
| Time To Live.                                                                                                                                                      |
| dontfragment                                                                                                                                                       | `DontFragment`         | `bool`              | No       | false                |
| Sets "Don't fragment flag" in IP packet.                                                                                                                           |
| dnsinterval                                                                                                                                                        | `DnsQueryInterval`     | `int`               | No       | 60000                |
| Time [milliseconds] between unsuccessful try and new try to obtain host's IP address using DNS.                                                                    |
| recenthistorydepth                                                                                                                                                 | `RecentHistoryDepth`   | `int`               | No       | 10                   |
| Number of sent pings used for recent history.                                                                                                                      |

Additional options are available for data series that represent source for showing hosts' statistics in charts \[these options are not required by `HostPinger` class\]. Series options should be stored in `data` node of host's node. Two options are currently available: `name` which represents name under which the series will appear in graph and `depth` which defines number of values that series will store. If `0` is specified for series depth, it will be unlimited. Each series must specify its source, by providing id of data source:

| Source ID | Source Name/Description         |
|-----------|---------------------------------|
| 0         | Received Packets Percent        |
| 1         | Lost Packets Percent            |
| 2         | Consecutive Lost Packets        |
| 3         | Received Packets Recent         |
| 4         | Received Packets Recent Percent |
| 5         | Lost Packets Recent             |
| 6         | Lost Packets Recent Percent     |
| 7         | Current Response Time           |
| 8         | Average Response Time           |
| 9         | Host Availability Percent       |

Several macros are available for naming data series: `%hostname%` \[name of the host\], `%ip%` \[IP address of the host\] and `%series%` \[name of data source listed in precious table\].

The application uses _hosts.cfg_ file located in the same directory as executable file to store list of hosts for pinging and ping options in XML format.

```xml
<pinger>

    <!-- List of host -->

    <host>

        <!-- Host options -->

    <name><!-- host name --></name>
    <ip><!-- host ip --></ip>
    <timeout><!-- timeout --></timeout>
    <interval><!-- ping interval --></interval>
    <pingsbeforedead><!-- pings before dead --></pingsbeforedead>
    <buffersize><!-- echo message size --></buffersize>
    <ttl><!-- Time To Live --></ttl>
    <dontfragment><!-- Don't fragment flag --></dontfragment>
    <description><!-- Description of the host --></description>
    <dnsinterval><!-- Duration of interval between DNS queries --></dnsinterval>
    <recenthistorydepth><!-- Depth of recent history --></recenthistorydepth>
    <data>
        <series source="source-id">
            <name><!-- Name of the series --></name>
            <depth><!--Depth of the series --></depth>
        </series>
    </data>

    </host>

    <host>

        <!-- Host options -->
        <!-- ... -->

    </host>

    <!-- More host... -->

</pinger>
```

Example:

```xml
<pinger>

  <host>
    <name>localhost</name>
    <ip>127.0.0.1</ip>
    <timeout>300</timeout>
    <interval>2000</interval>
    <pingsbeforedead>5</pingsbeforedead>
    <buffersize>32</buffersize>
    <ttl>3</ttl>
    <dontfragment>false</dontfragment>
  </host>

  <host>
    <name>google.com</name>
    <timeout>2000</timeout>
    <interval>2000</interval>
  </host>

  <host>
    <name>some local host</name>
    <ip>192.168.0.1</ip>
    <timeout>500</timeout>
    <interval>2000</interval>
  </host>

  <host>
    <name>yahoo.com</name>
    <timeout>2000</timeout>
    <interval>2000</interval>
  </host>

</pinger>
```

IP Scanning
-----------

<img width="486" height="535" alt="image" src="https://github.com/user-attachments/assets/c6b91ca1-119c-4cf4-bf3a-69d154183d87" />

IP scanning is a process of discovering active host in a specified range of IP addresses. It is a simple process, IP scanner just needs to send ICMP messages to all IP addresses in the range and wait for reply. Those addresses from which replies are returned within given timeframe are alive and all others are assumed to be dead. In order to avoid network overload, ICMP flood should be controlled by specifying how many concurrent pings there can be, when the number is reached scanner won't send more ICMP messages until previous ping requests are completed.

### IPScanner Class

`IPScanner` class implements scanning process. In addition to standard ICMP options \[described previously\], its constructors also accept number of concurrent pings that are allowed, how many pings are sent for each IP address while scanning and whether the scanning should loop after it reached the end of the range:

```csharp
public IPScanner(int concurrentPings, int pingsPerScan, bool continuousScan);
public IPScanner(int concurrentPings, int pingsPerScan, bool continuousScan,
    int timeout);
public IPScanner(int concurrentPings, int pingsPerScan, bool continuousScan,
    int timeout, int ttl, bool dontFragment, int pingBufferSize);
```

These settings can later be changed using appropriate properties, but not while the scanning process is active. `Active` property indicates whether the scanner is running.

Control over process of scanning is exposed through `Start`, `Stop` and `Wait` methods. `Wait` method blocks thread which called it until the scanning process is completed or aborted. Also the class has events that notify user when state of scanning process is changed: `OnStartScan`, `OnStopScan` and `OnRestartScan`.

`Start` method accepts IP address range as its parameter. Range is represented by `IPScanRange` class. Constructors accept either IP range \[the first and the last IP address\] or subnet \[IP address of the network and subnet mask\].

```csharp
IPScanRange(IPAddress start, IPAddress end);
IPScanRange(IPAddress start, int subnet);
```

This class also provides additional services, like calculating number of addresses in the range, comparing IP addresses, getting successive address and calculating distance between the addresses.

`OnScanProgressUpdate` event is raised each time after the scanner finishes with and IP address which provides method of tracking progress to users.

List of found hosts are available through `AliveHosts` property and when alive host is discovered scanner raises `OnAliveHostFound` event to notify user. Each host is represented by `IPScanHostState` class. The class stores various information and statistics about host which are discovered during the scanning process:

| Property Name     | Description                                                                         |
|-------------------|-------------------------------------------------------------------------------------|
| `Address`         | IP address of the host                                                              |
| `ResponseTimes`   | Array which contains response times of each ping request issued during the scanning |
| `PingsDone`       | Number of ping requests issued to the host                                          |
| `LossCount`       | Number of ping requests to which there was no response within required timeframe    |
| `AvgResponseTime` | Average response time                                                               |
| `Quality`         | Value that represents quality of connection to the host in range (0, 1)             |
| `QualityCategory` | Discrete value of connection quality                                                |
| `Address`         | IP address of the host                                                              |
| `CurrentState`    | Current state of the host: Testing, Dead or Alive                                   |
| `HostName`        | Name of the host discovered using reverse DNS lookup                                |

`IPScanHostState` class also provides methods for testing current state: `IsAlive`, `IsDead` and `IsTesting`. There is `OnStateChange` event which is raised when the state of the host is changed.

Tracerout
---------

<img width="537" height="479" alt="image" src="https://github.com/user-attachments/assets/69a54307-7568-4ecc-a4ae-36440de7d120" />

The purpose of tracerout is to show network path between source and destination host. It works by controlling TTL field. TTL defines maximal number of hops allowed before the packed should be discarded by the router. When router encounters such a packet, it sends ICMP Time Exceeded response to source which also includes IP address of the router. So at first, TTL is limited to one, and each time Time Exceeded response is received, traceroute print routers address, increases TTL and send another packet. The process is repeated until the destination host is reached. If Time Exceeded is not received after one or more tries, traceroute should skip the hop and go to the next by increasing TTL.

IPRouteTracer Class
-------------------

Traceroute is implemented by `IPRouteTracer` class and each hop \[router\] by `IPRouteHop` class.

Hop class contains IP address of the router, its ordinal number in the path and response times of each ping request.

`IPRouteTracer`'s constructors accept IP address of the destination host and various parameters: request timeout period, number of pings and number of retries for each hop. In each tracer, try sends specified number of ping request before it starts another try or progresses to the next hop. List of detected hops is exposed by `Route` property.

`Start`, `Stop` and `Wait` method provide control over tracing process. Also several events are available to track progress:

| Property Name      | Description                                                                                        |
|--------------------|----------------------------------------------------------------------------------------------------|
| `OnHopSuccess`     | Raised after the hop has been successfully traced [router responded to ping request at least once] |
| `OnHopPing`        | Raised when tracer receive response or request timeoutes                                           |
| `OnHopFail`        | Raised if the router has not responded to any of the sent ping request                             |
| `OnTraceStarted`   | Raised when tracing process is started                                                             |
| `OnTraceCompleted` | Raised after the tracing process is completed or aborted.                                          |


Charts
------

<img width="568" height="589" alt="image" src="https://github.com/user-attachments/assets/e51a3501-7ced-41e9-9fa6-a8d8e941b94d" />

The demo application can display statistics of monitored hosts as chart, but the code that implements them won't be discussed since there are much better articles that describe the subject.
