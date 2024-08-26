using System.Collections;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using UnityEngine;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

public class InAppPurchaseManager : MonoBehaviour,IDetailedStoreListener
{
    public string goldProductID = "com.DefaultCompany.Online-Services.Gold1";
    public string diamondProductID = "com.DefaultCompany.Online-Services.Diamond1";
    public string phantomProductID = "com.DefaultCompany.Online-Services.Phantom";

    [SerializeField] private Text goldScoreText;
    [SerializeField] private Text diamondScoreText;
    [SerializeField] private Text skinStatusText;

    private int goldScore;
    private int diamondScore;

    private string skinStatus;

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Initialize Sucess");
        storeController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error,"InitializedFailed");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("Initialize Failed");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("Purchase Failed");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        OnPurchaseFailed(product, PurchaseFailureReason.UserCancelled);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;

        if (product.definition.id == goldProductID)
        {
            AddGold();
        }

        else if (product.definition.id == diamondProductID)
        {
            AddDiamond();
        }

        else if (product.definition.id == phantomProductID)
        {
            AddPhantom();
        }

        return PurchaseProcessingResult.Complete;
    }

    public void AddPhantom()
    {
        skinStatus = "Skin Purchased";
        skinStatusText.text = skinStatus;
        PlayerPrefs.SetString("BuySkin", skinStatus);
    }

    public void AddGold()
    {
       // Debug.Log("Gold Added" + goldScore);
        goldScore += 50;
        Debug.Log("Gold score" + goldScore);
        goldScoreText.text = goldScore.ToString();
        PlayerPrefs.SetInt("Gold", goldScore);
    }

    public void AddDiamond()
    {
        Debug.Log("Diamond Added");
        diamondScore += 50;
        diamondScoreText.text = diamondScore.ToString();
        PlayerPrefs.SetInt("Diamond", diamondScore);
    }

    IStoreController storeController;

    public void InitializePurchase()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(goldProductID, ProductType.Consumable);
        builder.AddProduct(diamondProductID, ProductType.Consumable);
        builder.AddProduct(phantomProductID, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }
    public void CheckNonConsumablePurchase()
    {
        if (storeController == null)
        {
            Debug.Log("Null");
        }
        else
        {
            Product product = storeController.products.WithID(phantomProductID);
            if (product!= null && product.hasReceipt)
            {
                skinStatusText.text = "Skin Already Purchased";
                Debug.Log("You have Purchased");
            }
            else
            {
                Debug.Log("You Have Not Purchased");
            }
        }
    }

    public void GoldPurchase()
    {
        storeController.InitiatePurchase(goldProductID);
    }

    public void DiamondPurchase()
    {
        storeController.InitiatePurchase(diamondProductID);
    }

    public void PhantomPurchase()
    {
        storeController.InitiatePurchase(phantomProductID);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializePurchase();

        goldScore = PlayerPrefs.GetInt("Gold");
        goldScoreText.text =  goldScore.ToString();

        diamondScore = PlayerPrefs.GetInt("Diamond");
        diamondScoreText.text = diamondScore.ToString();

        skinStatus = PlayerPrefs.GetString("BuySkin");
        skinStatusText.text = skinStatus;

        CheckNonConsumablePurchase();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
