# IP Log Parser
Приложение для парсинга логов входящих запросов.

Формат лог-строки:
```
<IPv4>:yyyy-MM-dd HH:mm:ss
```
Пример лог файла
```
52.86.205.118:2023-11-10 08:40:00
203.0.113.15:2023-11-11 08:45:15
198.51.100.42:2023-11-11 08:50:30
142.250.184.238:2023-11-12 08:55:45
203.0.113.15:2023-11-13 09:00:00
52.86.205.118:2023-11-14 09:05:15
```

## Входные параметры

Параметры можно задать через коммандную строку:

```
--file-log=<path> [REQUIRED]
--file-output=<path> [REQUIRED]
--address-start=<IPv4>
--address-mask=<CIDR mask>
--time-start=dd.MM.yyyy
--time-end=dd.MM.yyyy
```
Либо через JSON конфигурацию:
``` json
{
    "file-log": "input.log",
    "file-output": "output.log",
    "address-start": "192.168.0.0",
    "address-mask": 16,
    "time-start": "01.04.2024",
    "time-end": "30.04.2024"
}
```

Либо через переменные среды(ENV), с такими же именами как выше.

Приоритет входных аргументов.
```
CommandLine > JSON > ENV
```
* Если в ENV задан параметр, его можно переписать в JSON, и еще раз через параметр коммандной строки.

## Что делает
Сопоставляет IP-адреса и количество запросов от них из **--file-log** в указынный период времени(**--time-start** и **--time-end**).
Так же фильтрует IP-адреса в заданных границах, от **--address-start** до последнего адреса в его подсети(**--address-mask**).

Результат записывается в выходной файл **--file-output** в формате:
```
<IPv4>:<Кол-во запросов>
```

Пример:
```
147.116.141.52:5
136.30.234.26:1
```