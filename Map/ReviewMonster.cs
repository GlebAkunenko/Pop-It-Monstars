using Google.Play.Review;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReviewMonster : Popit
{
    [SerializeField]
    private SpriteRenderer mouth;

    [SerializeField]
    private Sprite[] smiles;

    [SerializeField]
    private Animation reviewButton;

    private int s = 0;
    private bool startOpen;

    private void Start()
    {
        Interactable.CurrentMode = Interactable.Mode.game;

        OnDimpleCountChange += Smile;
    }

    private void Smile()
    {
        mouth.sprite = smiles[s++];
        if (s == smiles.Length)
            OnDimpleCountChange -= Smile;
    }

    protected override void OnFull()
    {
        reviewButton.Play();
        //GoToReview();
    }
    
    public void GoToReview()
    {
        //if (!startOpen)
        //    StartCoroutine(ReviewWindowOpening());

        Application.OpenURL("https://play.google.com/store/apps/details?id=com.JoppaGames");
        MetaSceneDate.OptionsData.Reviewed = true;
    }

    public void GoBack()
    {
        MetaSceneDate.SaveData();
        SceneManager.LoadScene(MetaSceneDate.GameData.CurrentLocation.Name);
    }

    private IEnumerator ReviewWindowOpening()
    {
        startOpen = true;

        ReviewManager reviewManager = new ReviewManager();
        var requestFlowOperation = reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError) {
            StartCoroutine(ReviewWindowOpening());
            yield break;
        }
        PlayReviewInfo playReviewInfo = requestFlowOperation.GetResult();

        var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
        yield return launchFlowOperation;
        playReviewInfo = null;
        if (launchFlowOperation.Error != ReviewErrorCode.NoError) {
            StartCoroutine(ReviewWindowOpening());
            yield break;
        }

        MetaSceneDate.OptionsData.Reviewed = true;
        startOpen = false;
    }
}
