using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Netcode;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public class NotifyClient : NetworkBehaviour
{
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject lightning;
    [SerializeField] private GameObject rock;
    private PlayerData clientData;
    private PlayerShoot playerShoot;
    private NotifyServer server;

    void Start()
    {
        clientData = GameObject.FindWithTag(TagManager.GameInfo).GetComponent<PlayerData>();
        playerShoot = GameObject.FindWithTag(TagManager.GameLogic).GetComponent<PlayerShoot>();
        server = GameObject.FindWithTag(TagManager.GameLogic).GetComponent<NotifyServer>();
    }

    [ClientRpc]
    public void JoinGameClientRpc()
    {
        if (!IsOwner)
            return;

        EnableSpellShootingScript();
        StartGameAr.StartNewGame();

        // if the dragon exists in scene already just start the game 
        SwapScreens.Instance.ToggleGameBackgroundClient();
        GameObject dragon = ScreenToggle.FindGameObjectWithTag(TagManager.BossParent);
        if (dragon != null)
        {
            ToggleOffPlacementText();
            SwapScreens.Instance.ToggleSpellBarOn();
        }
        else
        {
            GameScreen.Instance.DisplayPlacementText();
        }

        NotifyClientsUserHealth();
    }

    [ClientRpc]
    public void PartyLoseGameClientRpc()
    {
        if (!IsOwner)
            return;

        SwapScreens.Instance.ToggleLossScreen();
    }

    [ClientRpc]
    public void PartyWinGameClientRpc(string rewardsDictJson)
    {
        if (!IsOwner)
            return;

        Enemy bossScript = ScreenToggle.FindGameObjectWithTag(TagManager.BossParent).GetComponent<Enemy>();
        // have the boss play its death animation first
        StartCoroutine(bossScript.PlayBossDeath(2f, () => WinGameScreen(rewardsDictJson)));
    }

    [ClientRpc]
    public void PlayerRestartGameClientRpc()
    {
        if (!IsOwner)
            return;

        clientData.ResetGame();
        NotifyClientsUserHealth();

        GameObject[] puddles = GameObject.FindGameObjectsWithTag(TagManager.Puddle);
        foreach (GameObject puddle in puddles)
        {
            Destroy(puddle);
        }

        if (!IsHost)
            SwapScreens.Instance.ToggleResetGameClient();
    }

    [ClientRpc]
    public void PlayerGameStartedClientRpc()
    {
        if (!IsOwner)
            return;

        SwapScreens.Instance.ToggleGameBackgroundClient();
        ToggleOffPlacementText();
        SwapScreens.Instance.ToggleSpellBarOn();

        NotifyClientsUserHealth();
    }

    [ClientRpc]
    public void OtherPlayerHealthChangeClientRpc(string roomsJson)
    {
        if (!IsOwner)
            return;

        GameObject partyObj = GameObject.FindWithTag(TagManager.PartyHealth);

        // don't change the health bars if not yet instantiated
        if (partyObj == null)
            return;

        HealthUiUpdate partyHealthScript = partyObj.GetComponent<HealthUiUpdate>();
        partyHealthScript.UpdateHealthBars(roomsJson);
    }

    [ClientRpc]
    public void BossAttackPlayersClientRpc(string bossAttack)
    {
        if (!IsOwner)
            return;

        Enemy bossScript = ScreenToggle.FindGameObjectWithTag(TagManager.BossParent).GetComponent<Enemy>();

        switch (bossAttack)
        {
            case "Roar":
                bossScript.BossRoarAttack();
                break;
            case "GroundPound":
                bossScript.BossGroundPoundAttack();
                break;
            case "Fireball":
                bossScript.BossFireballAttack();
                break;
        }
    }

    [ClientRpc]
    public void SpawnOtherPlayerSpellClientRpc(Vector3 spawnPos, Vector3 direction, string casterUsername, string spell)
    {
        if (!IsOwner || clientData.GetUsername() == casterUsername)
            return;

        switch (spell)
        {
            case "fireball":
                GameObject fireballRep = Instantiate(fireball, spawnPos, Quaternion.LookRotation(direction));
                fireballRep.GetComponent<IPlayerSpell>().SetDamage(0);
                fireballRep.GetComponent<Rigidbody>().velocity = direction * 8f;
                break;
            case "lightning":
                GameObject lightningRep = Instantiate(lightning, spawnPos, Quaternion.LookRotation(direction) * Quaternion.Euler(80, 0, 0));
                lightningRep.GetComponent<IPlayerSpell>().SetDamage(0);
                break;
            case "rock":
                GameObject rockRep = Instantiate(rock, spawnPos, Quaternion.LookRotation(direction));
                rockRep.GetComponent<IPlayerSpell>().SetDamage(0);
                rockRep.GetComponent<Rigidbody>().velocity = direction * 8f;
                break;
        }
    }

    private void WinGameScreen(string rewardsDictJson)
    {
        Dictionary<string, int> rewards = JsonConvert.DeserializeObject<Dictionary<string, int>>(rewardsDictJson);
        var reward = rewards[clientData.GetUsername()];

        SwapScreens.Instance.ToggleWinScreen(reward);
    }

    private void ToggleOffPlacementText()
    {
        SwapScreens.Instance.ToggeOnCrosshair();
        GameScreen.Instance.SetPlaceTextEmpty();
    }

    private void EnableSpellShootingScript()
    {
        if (playerShoot == null)
        {
            Debug.Log("Unable to find shoot script");
            return;
        }

        playerShoot.enabled = true;
        return;
    }

    private void NotifyClientsUserHealth()
    {
        // send a request to all other clients so they can show the hp of this user
        if (server != null)
            server.NotifyClientHealthServerRpc(clientData.GetUsername(), clientData.GetHealth());
    }
}
