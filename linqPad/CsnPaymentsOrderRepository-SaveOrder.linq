<Query Kind="Program">
  <Reference>C:\csn\psi\Payments.Api\Csn.Psi.Payments.Api\bin\Debug\netcoreapp2.0\Csn.Psi.Payments.Api.dll</Reference>
  <NuGetReference>Csn.Dto</NuGetReference>
  <NuGetReference>Dapper</NuGetReference>
  <NuGetReference>Elasticsearch.Net</NuGetReference>
  <NuGetReference>NEST</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Csn.Psi.Payments.Api.Engine.Models</Namespace>
  <Namespace>Dapper</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

#region sql / json

private const string orderJson = @"
{
    'orderId': '929074CD-072F-46A7-AAAA-BA5C9D0C0001',
    'ownerId':'87fb3e3d-6675-49f6-8609-bd5ba871abcd',
    'service': 'chileautos',
    'application': 'PSI',
    'email': 'buyer@chile.com',
    'callBackUrl': 'http://sell-international.advert-api.csdev.com.au/v1/adverts/paid/f1c8ec9f-f777-7907-8a1a-7bc982af0009',
    'returnUrl': 'http://chileautos.csndev.com.au',
    'totalAmount': 1000.00,
    'items': [
      {
        'orderId':'929074CD-072F-46A7-AAAA-BA5C9D0C0001',
        'orderItemId': '929074CD-072F-46A7-BBBB-abcdefabcdef',
        'title': 'Standard Sell Your Car Package',
        'description': 'the standard package for selling online for cars in chile',
        'productType': 'Sell Your Car Online Package',
        'unitAmount': 1000.00,
        'quantity': 1,
        'totalAmount': 1000.00
      }
    ],
    'payments': []
  }
";

private const string InsertOrderSql = @"
UPDATE	[dbo].[Orders]
SET [OwnerId] = @OwnerId
	, [Service] = @Service
	, [Application] = @Application
	, [Email] = @Email
	, [CallBackUrl] = @CallBackUrl
	, [ReturnUrl] = @ReturnUrl
	, [TotalAmount] = @TotalAmount
	, [Status] = @Status
	, [PaymentMethod] = @PaymentMethod
	, [PaymentProvider] = @PaymentProvider
	, [ErrorMsg] = @ErrorMsg
	, [FailReason] = @FailReason
	, [DateCompletedUtc] = @DateCompletedUtc
	, [DateRefundedUtc] = @DateRefundedUtc
	, [DateInsertedUtc] = @DateInsertedUtc
	, [DateLastModifiedUtc] = @DateLastModifiedUtc
WHERE [OrderId] = @OrderId;
";

private const string UpdateOrderSql = @"
INSERT INTO [dbo].[Orders]
(
	[OrderId]
	, [OwnerId]
	, [Service]
	, [Application]
	, [Email]
	, [CallBackUrl]
	, [ReturnUrl]
	, [TotalAmount]
	, [Status]
	, [PaymentMethod]
	, [PaymentProvider]
	, [ErrorMsg]
	, [FailReason]
	, [DateCompletedUtc]
	, [DateRefundedUtc]
	, [DateInsertedUtc]
	, [DateLastModifiedUtc]
)
VALUES
(
	@OrderId
	, @OwnerId
	, @Service
	, @Application
	, @Email
	, @CallBackUrl
	, @ReturnUrl
	, @TotalAmount
	, @Status
	, @PaymentMethod
	, @PaymentProvider
	, @ErrorMsg
	, @FailReason
	, @DateCompletedUtc
	, @DateRefundedUtc
	, @DateInsertedUtc
	, @DateLastModifiedUtc
);";

private const string InsertOrdersItemSql = @"
INSERT INTO [dbo].[OrdersItems]
(
	[OrderItemId]
	, [OrderId]
	, [Title]
	, [Description]
	, [ProductType]
	, [UnitAmount]
	, [Quantity]
	, [TotalAmount]
	, [DateInsertedUtc]
	, [DateLastModifiedUtc]
)
VALUES
(
	@OrderItemId
	, @OrderId
	, @Title
	, @Description
	, @ProductType
	, @UnitAmount
	, @Quantity
	, @TotalAmount
	, @DateInsertedUtc
	, @DateLastModifiedUtc
);";

private const string DeleteAllOrdersItemsSql = "Delete From OrdersItems Where OrderId = @OrderId";

#endregion
private async Task<bool> OrderExists(Guid paymentOrderId)
{
	try
	{
		using (var connection = new SqlConnection("server=.;user id=latam_admin;password=00ibW3r7CHlMgvAabKjl;database=CsnGlobalPayments"))
		{
			const string sql = "select * from orders where orderid=@OrderId";
			Task<OrderModel> orderModel = connection.QuerySingleOrDefaultAsync<OrderModel>(sql, new { OrderId = paymentOrderId });
			return await orderModel != null;
		}
	}
	catch (InvalidOperationException exception)
	{
		string message = exception.Message.Contains("Sequence")
			? $"There are duplicate Orders records for Guid{paymentOrderId}"
			: $"Unable to fetch order with Guid {paymentOrderId} to see if it exits";

		throw;
	}
}
void Main()
{
	var tampered = orderJson.ToLower();
	var order = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderModel>(tampered);
	string sql;
	if (OrderExists(order.OrderId).Result)
	{
		sql = InsertOrderSql;
	}
	else
	{
		sql = UpdateOrderSql;
	}
	sql.Dump();
}

// Define other methods and classes here
