using UnityEngine;



public class TriggerEnvironmentDamage : MonoBehaviour
{
    GameObject waterSys;
    public Environment environment;
    bool ocean;
    bool swamp;
    bool spike;

    public void Start()
    {
        SwitchEnvironment(environment);
    }
    public enum Environment { Ocean, Swamp, Spikes }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera") && ocean)
        {
            waterSys = GameObject.FindGameObjectWithTag("Head");
            UnderWaterSystem waterSystem = waterSys.GetComponent<UnderWaterSystem>();
            waterSystem.StartSwimming(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Head") && ocean)
        {
            waterSys = GameObject.FindGameObjectWithTag("Head");
            UnderWaterSystem waterSystem = waterSys.GetComponent<UnderWaterSystem>();
            waterSystem.AboveWater(true);
        }
      
        else if (other.gameObject.CompareTag("Player") && swamp)
        {
            PlayerSystem playerSystem = other.gameObject.GetComponent<PlayerSystem>();
            playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.swamped, true);
        }
        else if (other.gameObject.CompareTag("Player") && spike)
        {
            PlayerSystem playerSystem = other.gameObject.GetComponent<PlayerSystem>();
            playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.spiked, true);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemySystem enemySys = other.gameObject.GetComponent<EnemySystem>();
            if (!enemySys.isToad)
                enemySys.DamageEnemy(100);
        }
        if (other.gameObject.CompareTag("Torch") && ocean)
        {
            TorchSystem torch = other.gameObject.GetComponent<TorchSystem>();
            if(torch.fixedTorch)
                torch.ActivateTorch(false);

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Head") && ocean)
        {
            waterSys = GameObject.FindGameObjectWithTag("Head");
            UnderWaterSystem waterSystem = waterSys.GetComponent<UnderWaterSystem>();
            waterSystem.StartSwimming(false);
            GameObject playerController = GameObject.FindGameObjectWithTag("Player");
            Quaternion setRot = new Quaternion(0, playerController.transform.rotation.y, 0, playerController.transform.rotation.w);
            playerController.transform.rotation = setRot;
        }
        else if(other.gameObject.CompareTag("Player") && swamp)
        {
            PlayerSystem playerSystem = other.gameObject.GetComponent<PlayerSystem>();
            playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.swamped, false);
        }
        else if (other.gameObject.CompareTag("Player") && spike)
        {
            PlayerSystem playerSystem = other.gameObject.GetComponent<PlayerSystem>();
            playerSystem.ApplyPlayerEffect(PlayerSystem.PlayerEffectStats.spiked, false);
        }
        if (other.gameObject.CompareTag("Torch") && ocean)
        {
            TorchSystem torch = other.gameObject.GetComponent<TorchSystem>();
            if (torch.fixedTorch)
                torch.ActivateTorch(true);

        }
    }
    public void SwitchEnvironment(Environment type)
    {
        switch (type)
        {
            case Environment.Ocean:
                {
                    ocean = true;
                    break;
                }
            case Environment.Swamp:
                {
                    swamp = true;
                    break;
                }
            case Environment.Spikes:
                {
                    spike = true;
                    break;
                }
        }
    }
}
