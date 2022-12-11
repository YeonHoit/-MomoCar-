import React from 'react'
import {StyleSheet, Modal, View, Text, Dimensions} from 'react-native'
import {TouchableView} from '../components'
import Color from 'color'
import {useNavigation} from '@react-navigation/native'
import {UnityModule} from '@asmadsen/react-native-unity-view'

const styles = StyleSheet.create({
  view: {
    flex: 1,
    backgroundColor: Color('#000000').alpha(0.45).string(),
    justifyContent: 'center'
  },
  alertBox: {
    backgroundColor: 'white',
    width: Dimensions.get('window').width - 80,
    marginLeft: 40,
    height: Dimensions.get('window').height / 4.5,
    borderRadius: 10,
    padding: 10
  },
  messageView: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center'
  },
  message: {
    fontSize: 15,
    color: 'black'
  },
  buttonView: {
    height: 35,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center'
  },
  okButton: {
    width: (Dimensions.get('window').width - 90) / 2 - 5,
    height: 35,
    backgroundColor: '#E6135A',
    alignItems: 'center',
    justifyContent: 'center',
    borderRadius: 5
  },
  okText: {
    color: 'white',
    fontSize: 14,
    fontWeight: 'bold'
  },
  cancelButton: {
    width: (Dimensions.get('window').width - 90) / 2 - 5,
    height: 35,
    backgroundColor: '#F1F1F5',
    alignItems: 'center',
    justifyContent: 'center',
    borderRadius: 5,
    marginRight: 5
  },
  cancelText: {
    color: '#999999',
    fontSize: 14,
    fontWeight: 'bold'
  }
})

export default function UnityModal() {
  const navigation = useNavigation()

  return (
    <Modal transparent={true} style={{flex: 1}}>
      <View style={[styles.view]}>
        <View style={[styles.alertBox]}>
          <View style={[styles.messageView]}>
            <Text style={[styles.message]}>가상튜닝을 시작하시겠습니까?</Text>
          </View>
        </View>
        <View style={[styles.buttonView]}>
          <TouchableView
            style={[styles.cancelButton]}
            onPress={() => navigation.canGoBack() && navigation.goBack()}>
            <Text style={[styles.cancelText]}>취소</Text>
          </TouchableView>
          <TouchableView
            style={[styles.okButton]}
            onPress={() => {
              UnityModule.createUnity().then((value) => {
                navigation.navigate('Unity')
              })
            }}>
            <Text style={[styles.okText]}>확인</Text>
          </TouchableView>
        </View>
      </View>
    </Modal>
  )
}
