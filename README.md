OrderingService servisine CompleteOrderAsync metodu eklendi. Bu metod ile OrdersApi’deki /complete endpointine 
istek atılmaktadır. CompleteOrderAsync api metodu üzerinden işlem devam eder. Burada requestId validasyonu 
yapıldıktan sonra CompleteOrderCommand nesnesi oluşturulur. Bu nesne kullanılarak IdentifiedCommand nesnesi oluşturulur 
ve Mediator kullanılarak gönderilir. Bu aşamada IdentifiedCommandHandler içinde requestId kontrolü yapılır. 
Aynı requestId ile istek atıldığında log yazılması sağlanmıştır ve akışın devam etmesi engellenmiştir. 
Bunun yanısıra hata da fırlatılabilirdi. Bu kontrolü geçtik sonra verilen command, Mediator ile gönderilir 
ve ilk olarak  CompleteOrderCommandValidator çalışır. 

Buradaki validasyonlardan geçtikten sonra CompleteOrderCommandHandler tarafından yakalanır. 
Burada orderNumber ile Order bulunur ve DDD prensiplerine göre yazılan SetCompletedStatus metodu çağrılır. 
Bu metodta ilk önce Order’ın statü kontrolü yapılır. Statüsü Shipped değilse hata fırlatılır. 
Shipped ise statüsü güncellenerek Complete’e çekilir. Daha sonra da OrderCompletedDomainEvent oluşturulur. 
Bu event OrderCompletedDomainEventHandler tarafından yakalanır.Burada Order ve Buyer bilgileri ile 
OrderStatusChangedToCompletedIntegrationEvent oluşturulur. Bu event bilgileri ile de IntegrationEventLogEntry 
tablosuna kayıt atılır. Oluşturulan integration eventini OrderStatusChangedToCompletedIntegrationEventHandler 
yakalar ve notification gönderilir.

Ekstra olarak unit testler eklenmiştir.
Not: Order’ın Completed olabilmesi için öncesinde Shipped statüsüne çekilmesi gerekmektedir. 
