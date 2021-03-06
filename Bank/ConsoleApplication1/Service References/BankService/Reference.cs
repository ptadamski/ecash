﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sklep.BankService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BankService.IBankService", CallbackContract=typeof(Sklep.BankService.IBankServiceCallback))]
    public interface IBankService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/doInit")]
        void doInit(Common.Banknote aBanknote, bool aUnderCreation);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/doCreateAgreement")]
        void doCreateAgreement(string[] aBlindMessages);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/doVerifyAgreement")]
        void doVerifyAgreement(Common.PublicSecret[] aSecrets, string[] aBlindingFactors);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/doDepone")]
        void doDepone(Common.Secret aBanknote, string aSignature, int[] aIdIndexList, Common.PrivateSecret[] aPartialIdList);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBankServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/onInit")]
        void onInit(Common.Banknote aBanknote, int aBanknoteCount, string aPublicKey);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/onCreateAgreement")]
        void onCreateAgreement(int aIndex);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/onVerifyAgreement")]
        void onVerifyAgreement(Common.PublicSecret aBanknote, string aBlindSignature, bool aAgreed);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBankServiceChannel : Sklep.BankService.IBankService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BankServiceClient : System.ServiceModel.DuplexClientBase<Sklep.BankService.IBankService>, Sklep.BankService.IBankService {
        
        public BankServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public BankServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public BankServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public BankServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public BankServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void doInit(Common.Banknote aBanknote, bool aUnderCreation) {
            base.Channel.doInit(aBanknote, aUnderCreation);
        }
        
        public void doCreateAgreement(string[] aBlindMessages) {
            base.Channel.doCreateAgreement(aBlindMessages);
        }
        
        public void doVerifyAgreement(Common.PublicSecret[] aSecrets, string[] aBlindingFactors) {
            base.Channel.doVerifyAgreement(aSecrets, aBlindingFactors);
        }
        
        public void doDepone(Common.Secret aBanknote, string aSignature, int[] aIdIndexList, Common.PrivateSecret[] aPartialIdList) {
            base.Channel.doDepone(aBanknote, aSignature, aIdIndexList, aPartialIdList);
        }
    }
}
