# Archiver

Написать консольную программу на C#, предназначенную для поблочного сжатия и
расжатия файлов с помощью System.IO.Compression.GzipStream.
Программа должна эффективно распараллеливать и синхронизировать обработку блоков
в многопроцессорной среде и уметь обрабатывать файлы, размер которых превышает
объем доступной оперативной памяти.
В случае исключительных ситуаций необходимо проинформировать пользователя
понятным сообщением.
При работе с потоками допускается использовать только стандартные классы и
библиотеки из .Net 3.5 (исключая ThreadPool, BackgroundWorker, TPL). Ожидается
реализация с использованием Thread-ов.
Код программы должен соответствовать принципам ООП и ООД (читаемость, разбиение
на классы и т.д.).
Параметры программы, имена исходного и результирующего файлов должны задаваться
в командной строке следующим образом:
GZipTest.exe compress/decompress [имя исходного файла] [имя результирующего файла]
Исходники необходимо прислать вместе с проектом Visual Studio.

--------------------------------------------------------------------------------------------------------------------
1. Нет разбиения кода на классы с выделенными ролями/ответственностями.

2. Потоки постоянно пересоздаются, что крайне не эффективно.

3. Нет обработки ошибок в потоках.

4. Правильный порядок блоков при декомпрессии получился только благодаря схеме ожидания завершения работы потоков, 
которая вносит задержки в систему. Не используются примитивы синхронизации, образуются глухие циклы с проверкой состояний потоков.

---------------------------------------------------------------------------------------------------------------------

 Программа будет оцениваться по следующим критериям:
1.  Работоспособность – проверяется на тестовых файлах с размерами от 0 до 32 Gb
2.  Правильность выбора алгоритма с точки зрения эффективности – должен быть максимально загружен самый слабый компонент системы (диск/процессор)
3.  Знание и умение использовать примитивы синхронизации – должны быть правильно выбраны примитивы для синхронизации потоков, доступа к данным
4.  Проработка архитектуры – есть разбиение на классы по принципам ООП и ООД, не должно быть лишних классов, интерфейсов, методов и т.д.
5.  Читабельность и понятность кода – код должен быть простым, аккуратным; алгоритм программы должен быть понятен без отладки
6.  Грамотная обработка ошибок и нестандартных ситуаций – должна выводиться диагностическая информация, по которой должно быть понятно что произошло без отладки программы.
7.  Правильное управления ресурсами – не должно быть утечек неуправляемых ресурсов, а также своевременное уничтожение управляемых ресурсов.
