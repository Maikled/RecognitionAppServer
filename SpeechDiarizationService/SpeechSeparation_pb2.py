# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: SpeechSeparation.proto
# Protobuf Python Version: 5.26.1
"""Generated protocol buffer code."""
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import symbol_database as _symbol_database
from google.protobuf.internal import builder as _builder
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n\x16SpeechSeparation.proto\"B\n\x0eSeparationData\x12\r\n\x05start\x18\x01 \x01(\x01\x12\x0b\n\x03\x65nd\x18\x02 \x01(\x01\x12\x14\n\x0cspeaker_name\x18\x03 \x01(\t\"L\n\x0cResponseData\x12\x11\n\tfile_name\x18\x01 \x01(\t\x12)\n\x10separation_datas\x18\x02 \x03(\x0b\x32\x0f.SeparationData\" \n\x0bRequestData\x12\x11\n\tfile_path\x18\x01 \x01(\t2@\n\x10SpeechSeparation\x12,\n\rGetSeparation\x12\x0c.RequestData\x1a\r.ResponseDataB\x19\xaa\x02\x16SpeechSeparationServerb\x06proto3')

_globals = globals()
_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, _globals)
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'SpeechSeparation_pb2', _globals)
if not _descriptor._USE_C_DESCRIPTORS:
  _globals['DESCRIPTOR']._loaded_options = None
  _globals['DESCRIPTOR']._serialized_options = b'\252\002\026SpeechSeparationServer'
  _globals['_SEPARATIONDATA']._serialized_start=26
  _globals['_SEPARATIONDATA']._serialized_end=92
  _globals['_RESPONSEDATA']._serialized_start=94
  _globals['_RESPONSEDATA']._serialized_end=170
  _globals['_REQUESTDATA']._serialized_start=172
  _globals['_REQUESTDATA']._serialized_end=204
  _globals['_SPEECHSEPARATION']._serialized_start=206
  _globals['_SPEECHSEPARATION']._serialized_end=270
# @@protoc_insertion_point(module_scope)
