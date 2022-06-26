function CreateOrders(items) {

    var context = getContext();
    var response = context.getResponse();

    if (!items) {
        response.setBody("Error: Items are undefined!");
        return;
    }

    var numberOfItems = items.length;
    checkLenght(numberOfItems);

    function checkLenght(itemLenght) {
        if (itemLenght === 0) {
            response.setBody("Error: There are no items!");
        }
    }

    for (let i = 0; i < numberOfItems; i++) {
        createItem(items[i]);
    }

    function createItem(item) {
        var collection = context.getCollection();
        var collectionLink = collection.getSelfLink();
        collection.createDocument(collectionLink, item);
    }
}