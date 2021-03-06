﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Customer.Bank {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="BankNote", Namespace="http://schemas.datacontract.org/2004/07/Bank")]
    [System.SerializableAttribute()]
    public partial class BankNote : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private System.Guid SerialField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Customer.Bank.IdSeq UserIdentityField;
        
        private int ValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public System.Guid Serial {
            get {
                return this.SerialField;
            }
            set {
                if ((this.SerialField.Equals(value) != true)) {
                    this.SerialField = value;
                    this.RaisePropertyChanged("Serial");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Customer.Bank.IdSeq UserIdentity {
            get {
                return this.UserIdentityField;
            }
            set {
                if ((object.ReferenceEquals(this.UserIdentityField, value) != true)) {
                    this.UserIdentityField = value;
                    this.RaisePropertyChanged("UserIdentity");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int Value {
            get {
                return this.ValueField;
            }
            set {
                if ((this.ValueField.Equals(value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="IdSeq", Namespace="http://schemas.datacontract.org/2004/07/Bank")]
    [System.SerializableAttribute()]
    public partial class IdSeq : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string HashField;
        
        private System.Guid RandNumField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Hash {
            get {
                return this.HashField;
            }
            set {
                if ((object.ReferenceEquals(this.HashField, value) != true)) {
                    this.HashField = value;
                    this.RaisePropertyChanged("Hash");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public System.Guid RandNum {
            get {
                return this.RandNumField;
            }
            set {
                if ((this.RandNumField.Equals(value) != true)) {
                    this.RandNumField = value;
                    this.RaisePropertyChanged("RandNum");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Bank.IBankService", CallbackContract=typeof(Customer.Bank.IBankServiceCallback))]
    public interface IBankService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/doCreate")]
        void doCreate(Customer.Bank.BankNote banknote);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/doCreate")]
        System.Threading.Tasks.Task doCreateAsync(Customer.Bank.BankNote banknote);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/doValidate")]
        void doValidate(string banknote, string signature);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/doValidate")]
        System.Threading.Tasks.Task doValidateAsync(string banknote, string signature);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/doAgreementInit")]
        void doAgreementInit(string[] blindedMessageList);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/doAgreementInit")]
        System.Threading.Tasks.Task doAgreementInitAsync(string[] blindedMessageList);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/doAgreementVerf")]
        void doAgreementVerf(string[] messageList, string[] blindingFactorList);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/doAgreementVerf")]
        System.Threading.Tasks.Task doAgreementVerfAsync(string[] messageList, string[] blindingFactorList);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBankServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/onBeforeAgreementInit")]
        void onBeforeAgreementInit(Customer.Bank.BankNote banknote, int count);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/onPublicKey")]
        void onPublicKey(string pubKey);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/onBeforeAgreementVerf")]
        void onBeforeAgreementVerf(int excludeFromAgreement);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/onAfterAgreementVerf")]
        void onAfterAgreementVerf(string blindSignature);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IBankService/onBankNoteValidate")]
        void onBankNoteValidate(string banknote, string signature, bool result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBankServiceChannel : Customer.Bank.IBankService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BankServiceClient : System.ServiceModel.DuplexClientBase<Customer.Bank.IBankService>, Customer.Bank.IBankService {
        
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
        
        public void doCreate(Customer.Bank.BankNote banknote) {
            base.Channel.doCreate(banknote);
        }
        
        public System.Threading.Tasks.Task doCreateAsync(Customer.Bank.BankNote banknote) {
            return base.Channel.doCreateAsync(banknote);
        }
        
        public void doValidate(string banknote, string signature) {
            base.Channel.doValidate(banknote, signature);
        }
        
        public System.Threading.Tasks.Task doValidateAsync(string banknote, string signature) {
            return base.Channel.doValidateAsync(banknote, signature);
        }
        
        public void doAgreementInit(string[] blindedMessageList) {
            base.Channel.doAgreementInit(blindedMessageList);
        }
        
        public System.Threading.Tasks.Task doAgreementInitAsync(string[] blindedMessageList) {
            return base.Channel.doAgreementInitAsync(blindedMessageList);
        }
        
        public void doAgreementVerf(string[] messageList, string[] blindingFactorList) {
            base.Channel.doAgreementVerf(messageList, blindingFactorList);
        }
        
        public System.Threading.Tasks.Task doAgreementVerfAsync(string[] messageList, string[] blindingFactorList) {
            return base.Channel.doAgreementVerfAsync(messageList, blindingFactorList);
        }
    }
}
