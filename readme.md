# Эмулятор простого биржевого робота.
 
Описание: 

Cистема состоит из нескольких частей:

1. Эмулятор цен с биржи по заданному инструменту
    - генерирует поток цен каждую секунду (точность по времени не важна)
    - сколько то секунд цена растет, затем падает или может не меняться
2. Торговая стратегия
    - Работает по скользящей средней (или по нескольким), алгоритм не важен – можно использовать любой.
    Например – средняя растет – покупаем, падает – продаем.
    Или еще проще – цена растет, покупаем, иначе – продаем.
    - Получает цены из эмулятора (1)
    - Параметры стратегии настраиваются, например, период средней – UI не нужен, конфига или настроек в коде достаточно
    - Выдает три типа сигналов – покупка, продажа или нет сигнала (держать позицию).
3. Эмулятор биржи - опционально
    - Принимает сигналы от стратегии и выводит на экран
    - Получает цены из эмулятора (1)
    - Покупает или продает инструмент по сигналу от стратегии и выводит на экран текущий баланс счета

По стеку – dotnet. UI не требуется.
Для экономии времени (1), (2), (3) можно представить как классы консольного приложения.

# Stock Exchange Emulator
1. StockExchangePrice class consists of 2 timers: _timerChangeBehaviour changes the direction of changing a price, _timerChangePrice changes a price. StockExchangePrice sends OnNext with the price to observers.
2. TradingStrategy subscribes for the StockExchangePrice, calls IStrategy.GetAction and sends OnNext with the action. IStrategy defines by using the fabric method of ICreator based on strategy name defined in appconfig.json. The strategy name  should correspond to DisplayAttribute for the StrategyType enum.
3. StockExchangeEmulator subscribes for both TradingStrategy & StockExchangePrice. StockExchangeEmulator decides what to do with stocks. 
4. Program print info to the console
